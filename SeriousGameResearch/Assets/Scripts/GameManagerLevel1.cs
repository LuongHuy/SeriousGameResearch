using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameManagerLevel1 : MonoBehaviour
{
    private List<Level1QuestionConfig.questionData> totalQuestion;
    public Level1QuestionConfig questionConfig;
    public TextMeshProUGUI question;
    public TextMeshProUGUI answer;
    public Button trueButton;
    public Button falseButton;

    public GameObject endGameUI;

    public GameObject trueText;
    public GameObject falseText;
    public GameObject panelDialog;

    private Coroutine currentTextCoroutine;
    private Level1QuestionConfig.questionData currentQuestionData;


    public AudioSource sfxSource;         
    public AudioClip correctSFX;         
    public AudioClip incorrectSFX;         
    public AudioClip endGameSFX;
    [Range(0f, 1f)] public float sfxVolume = 0.8f;

    private void Awake()
    {
        init();
    }

    private void Start()
    {
        DialogManager.Instance.ShowConversation("Level_1", OnDialogCompleted);
    }

    private void OnDialogCompleted()
    {
        showQuestion();

        if (DialogManager.Instance != null)
        {
            DialogManager.Instance.gameObject.SetActive(false);
            panelDialog.SetActive(false);
        }
    }

    private void init()
    {
        totalQuestion = new List<Level1QuestionConfig.questionData>(questionConfig.questionList);

        trueButton.onClick.RemoveAllListeners();
        falseButton.onClick.RemoveAllListeners();

        trueButton.onClick.AddListener(() => onClickToAnswer(true));
        falseButton.onClick.AddListener(() => onClickToAnswer(false));

        if (sfxSource != null)
        {
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
            sfxSource.spatialBlend = 0f; // 2D sound
        }
    }

    private void showQuestion()
    {
        if (totalQuestion.Count == 0)
        {
            Debug.Log("EndGame");
            trueButton.enabled = false;
            falseButton.enabled = false;

            if (endGameUI != null)
            {
                endGameUI.SetActive(true);
            }
            PlaySFX(endGameSFX);
            return;
        }

        var questionIndex = Random.Range(0, totalQuestion.Count);
        currentQuestionData = totalQuestion[questionIndex];
        totalQuestion.RemoveAt(questionIndex);

        question.text = currentQuestionData.question;
        answer.text = currentQuestionData.answer;
    }

    public void onClickToAnswer(bool isCorrect)
    {
        if (currentTextCoroutine != null)
        {
            StopCoroutine(currentTextCoroutine);
            trueText.SetActive(false);
            falseText.SetActive(false);
        }

        if (isCorrect == currentQuestionData.isCorrect)
        {
            Debug.Log("True");
            PlaySFX(correctSFX);
            currentTextCoroutine = StartCoroutine(ShowTextForSeconds(trueText, 0.75f));
        }
        else
        {
            Debug.Log("False");
            PlaySFX(incorrectSFX);
            currentTextCoroutine = StartCoroutine(ShowTextForSeconds(falseText, 0.75f));
        }

        showQuestion();
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.pitch = Random.Range(0.95f, 1.05f); // nghe tự nhiên hơn
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
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
}
