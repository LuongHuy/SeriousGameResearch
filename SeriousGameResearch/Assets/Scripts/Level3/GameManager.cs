using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private StreetManager streetManager;
    [SerializeField] private QuestionUIView questionUIView;
    [SerializeField] private QuestionConfig questionConfig;

    [Header("UI")]
    public GameObject winGameUI;
    public GameObject loseGameUI;
    public GameObject trueText;
    public GameObject falseText;
    public TextMeshProUGUI healthText;

    [Header("Audio / SFX Settings")]
    public AudioSource sfxSource;       // 2D, Loop=Off, PlayOnAwake=Off
    public AudioClip correctSFX;        // SFX khi trả lời đúng
    public AudioClip incorrectSFX;      // SFX khi trả lời sai
    public AudioClip endgameWinSFX;     // SFX khi chiến thắng (EndGame Win)
    public AudioClip endgameLoseSFX;    // SFX khi thua (EndGame Lose)
    [Range(0f, 1f)] public float sfxVolume = 0.8f;

    private Queue<QuestionConfig.QuestionData> questionQueue;
    private QuestionConfig.QuestionData currentQuestion;
    private float delayAnswer;
    private int curHealth;
    private Coroutine currentTextCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Cấu hình sfxSource chuẩn
        if (sfxSource != null)
        {
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
            sfxSource.spatialBlend = 0f; // 2D
        }
    }

    private void Start()
    {
        curHealth = 3;
        healthText.text = curHealth.ToString();

        DialogManager.Instance.ShowConversation("Level_3", OnConversationEnd);

        questionQueue = new Queue<QuestionConfig.QuestionData>(questionConfig.Questions);
        streetManager.InitStreet(
            questionConfig.Questions.Length,
            characterController.ForwardSpeed * questionConfig.TimePerQuestion,
            AnswerQuestion
        );
    }

    public void GenerateQuestion()
    {
        if (questionQueue.Count == 0)
        {
            ShowGameoverWin();
            return;
        }

        currentQuestion = questionQueue.Dequeue();
        questionUIView.ShowQuestion(currentQuestion.question);
    }

    public bool AnswerQuestion(bool isTrue)
    {
        if (currentQuestion == null || delayAnswer > Time.time)
        {
            Debug.LogWarning("No current question to answer.");
            return false;
        }

        if (currentQuestion.answer == isTrue)
        {
            // Đúng
            StartCoroutine(ShowTextForSeconds(trueText, 0.75f));
            PlaySFX(correctSFX);
        }
        else
        {
            // Sai
            StartCoroutine(ShowTextForSeconds(falseText, 0.75f));
            PlaySFX(incorrectSFX);

            curHealth--;
            healthText.text = curHealth.ToString();

            if (curHealth <= 0)
            {
                ShowGameoverLose();
                return false;
            }
        }

        bool result = currentQuestion.answer == isTrue;
        delayAnswer = Time.time + 0.1f;

        GenerateQuestion();
        return result;
    }

    private void OnConversationEnd()
    {
        characterController.CanMove = true;
        GenerateQuestion();
    }

    private void ShowGameoverWin()
    {
        characterController.CanMove = false;
        winGameUI.SetActive(true);

        // 🔊 Phát âm thanh thắng cuối màn
        PlaySFX(endgameWinSFX);
    }

    private void ShowGameoverLose()
    {
        characterController.CanMove = false;
        loseGameUI.SetActive(true);

        // 🔊 Phát âm thanh thua cuối màn
        PlaySFX(endgameLoseSFX);
    }

    private IEnumerator ShowTextForSeconds(GameObject textObj, float seconds)
    {
        if (textObj != null)
        {
            textObj.SetActive(true);
            yield return new WaitForSeconds(seconds);
            textObj.SetActive(false);
            currentTextCoroutine = null;
        }
    }

    // Hàm phát âm thanh chung
    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }
}
