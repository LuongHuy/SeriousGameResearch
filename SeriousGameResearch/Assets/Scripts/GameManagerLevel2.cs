using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerLevel2 : MonoBehaviour
{
    private List<Level2QuestionConfig.questionData> totalQuestion;
    public Level2QuestionConfig questionConfig;
    public TextMeshProUGUI question;
    public TextMeshProUGUI answer;
    public Button answer1, answer2, answer3, answer4;
    public TextMeshProUGUI answer1Name, answer2Name, answer3Name, answer4Name;

    public GameObject endGameUI;

    public GameObject trueText;
    public GameObject falseText;
    public GameObject panelDialog;

    private Coroutine currentTextCoroutine;

    private Level2QuestionConfig.questionData currentQuestionData;

    private void Awake()
    {
        init();
    }

    private void Start()
    {
        //showQuestion();
        DialogManager.Instance.ShowConversation("Level_2", OnDialogCompleted);
    }

    private void OnDialogCompleted()
    {
        // Show the first question
        showQuestion();

        // Disable the DialogManager GameObject
        if (DialogManager.Instance != null)
        {
            DialogManager.Instance.gameObject.SetActive(false);
            panelDialog.SetActive(false);
        }
    }

    private void init()
    {
        totalQuestion = new List<Level2QuestionConfig.questionData>(questionConfig.questionList);
        answer1.onClick.RemoveAllListeners();
        answer2.onClick.RemoveAllListeners();           
        answer3.onClick.RemoveAllListeners();        
        answer4.onClick.RemoveAllListeners();

        answer1.onClick.AddListener(() => onClickToAnswer(0));
        answer2.onClick.AddListener(() => onClickToAnswer(1));
        answer3.onClick.AddListener(() => onClickToAnswer(2));
        answer4.onClick.AddListener(() => onClickToAnswer(3));

    }

    private void showQuestion()
    {
        if (totalQuestion.Count == 0)
        {
            Debug.Log("EndGame");
            answer1.enabled = false;
            answer2.enabled = false;
            answer3.enabled = false;
            answer4.enabled = false;

            if (endGameUI != null)
            {
                endGameUI.SetActive(true);
            }
            return;
        }
        var questionIndex = Random.Range(0, totalQuestion.Count);
        currentQuestionData = totalQuestion[questionIndex];
        totalQuestion.RemoveAt(questionIndex);
        question.text = currentQuestionData.question;
        answer.text = currentQuestionData.answer;

        answer1Name.text = currentQuestionData.result[0];
        answer2Name.text = currentQuestionData.result[1];
        answer3Name.text = currentQuestionData.result[2];
        answer4Name.text = currentQuestionData.result[3];
    }

    public void onClickToAnswer(int correctIndex)
    {
        if (currentTextCoroutine != null)
        {
            StopCoroutine(currentTextCoroutine);
            trueText.SetActive(false);
            falseText.SetActive(false);
        }

        if (correctIndex == currentQuestionData.correctIndex)
        {
            Debug.Log("True");
            StartCoroutine(ShowTextForSeconds(trueText, 0.75f));
        }
        else
        {
            Debug.Log("False");
            StartCoroutine(ShowTextForSeconds(falseText, 0.75f));
        }
        showQuestion();
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
