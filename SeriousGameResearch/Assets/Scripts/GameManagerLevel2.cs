using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerLevel2 : MonoBehaviour
{
    private List<Level2QuestionConfig.questionData> totalQuestion;
    [Header("Config")]
    public Level2QuestionConfig questionConfig;

    [Header("Timing")]
    public float delayShowAnswerResult = 1f;

    [Header("UI")]
    public GameObject endGameUI;
    public GameObject panelDialog;
    public UIQuestionView questionView;
    public UIAnswerView[] uIAnswerViews;

    private Level2QuestionConfig.questionData currentQuestionData;
    private int questionPass;

    [Header("SFX Settings")]
    public AudioSource sfxSource;           // 2D, Loop=Off, PlayOnAwake=Off
    public AudioClip correctSFX;            // đúng
    public AudioClip incorrectSFX;          // sai
    public AudioClip endGameSFX;            // end game
    [Range(0f, 1f)] public float sfxVolume = 0.8f;

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        // Mở hội thoại đầu màn
        DialogManager.Instance.ShowConversation("Level_2", OnDialogCompleted);
    }

    private void OnDialogCompleted()
    {
        // Show the first question
        ShowQuestion();

        // Ẩn panel thoại
        if (DialogManager.Instance != null)
        {
            DialogManager.Instance.gameObject.SetActive(false);
            panelDialog.SetActive(false);
        }
    }

    private void Init()
    {
        totalQuestion = new List<Level2QuestionConfig.questionData>(questionConfig.questionList);

        // bind drag-drop events
        foreach (var uIAnswerView in uIAnswerViews)
        {
            questionView.OnDragQuestion += uIAnswerView.OnCheckPickFromQuestionView;
            questionView.OnEndDragQuestion += uIAnswerView.OnPickAnswer;
            uIAnswerView.OnPickAnswerHandle += OnClickToAnswer;
        }

        questionView.SetInteract(true);

        // Chuẩn hoá AudioSource SFX
        if (sfxSource != null)
        {
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
            sfxSource.spatialBlend = 0f; // 2D
        }
    }

    private void ResetAnswersAndQuestion()
    {
        questionView.transform.localPosition = Vector3.zero;
        questionView.SetInteract(false);

        foreach (var uIAnswerView in uIAnswerViews)
            uIAnswerView.ResetValue();
    }

    private void ShowQuestion()
    {
        ResetAnswersAndQuestion();

        if (totalQuestion.Count == 0)
        {
            Debug.Log("EndGame");

            if (endGameUI != null)
                endGameUI.SetActive(true);

            // 🔊 SFX EndGame
            PlaySFX(endGameSFX);
            return;
        }

        var questionIndex = Random.Range(0, totalQuestion.Count);
        currentQuestionData = totalQuestion[questionIndex];
        totalQuestion.RemoveAt(questionIndex);

        for (var i = 0; i < currentQuestionData.result.Length; i++)
        {
            if (i < uIAnswerViews.Length)
                uIAnswerViews[i].SetAnswert(currentQuestionData.result[i], i);
        }

        questionView.SetQuestion(currentQuestionData.question, currentQuestionData.answer, questionPass == 0);
        questionView.SetInteract(true);

        questionPass++;
    }

    public async void OnClickToAnswer(int pickedIndex)
    {
        questionView.SetInteract(false);
        var answerView = uIAnswerViews[pickedIndex];

        if (pickedIndex == currentQuestionData.correctIndex)
        {
            Debug.Log("True");
            answerView.PickResultForView(Color.green, true);
            PlaySFX(correctSFX);          // 🔊 đúng
        }
        else
        {
            Debug.Log("False");
            answerView.PickResultForView(Color.red, false);

            // highlight đáp án đúng
            foreach (var view in uIAnswerViews)
            {
                if (view.AnswerIndex == currentQuestionData.correctIndex)
                {
                    view.PickResultForView(Color.green, false);
                    break;
                }
            }
            PlaySFX(incorrectSFX);        // 🔊 sai
        }

        await Task.Delay((int)(delayShowAnswerResult * 1000));
        ShowQuestion();
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.pitch = Random.Range(0.95f, 1.05f); // tự nhiên hơn
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }
}
