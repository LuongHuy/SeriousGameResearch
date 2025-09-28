using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManagerLevel1 : MonoBehaviour
{
    private List<Level1QuestionConfig.questionData> totalQuestion;
    public Level1QuestionConfig questionConfig;
    public TextMeshProUGUI question;
    public TextMeshProUGUI answer;
    public Button trueButton;
    public Button falseButton;

    private Level1QuestionConfig.questionData currentQuestionData;

    private void Awake()
    {
        init();
    }

    private void Start()
    {
        // showQuestion();
        DialogManager.Instance.ShowConversation("Level_1",showQuestion);
    }

    private void init()
    {
        totalQuestion = new List<Level1QuestionConfig.questionData> (questionConfig.questionList);
        trueButton.onClick.RemoveAllListeners();
        falseButton.onClick.RemoveAllListeners();
        trueButton.onClick.AddListener(() => onClickToAnswer(true));
        falseButton.onClick.AddListener(() => onClickToAnswer(false));
    }

    private void showQuestion()
    {
        if (totalQuestion.Count == 0)
        {          
            Debug.Log("EndGame");
            trueButton.enabled = false;
            falseButton.enabled = false;
            return;
        }
        var questionIndex = Random.Range (0, totalQuestion.Count);
        currentQuestionData = totalQuestion[questionIndex];
        totalQuestion.RemoveAt (questionIndex);
        question.text = currentQuestionData.question;
        answer.text = currentQuestionData.answer;

    }

    public void onClickToAnswer(bool isCorrect)
    {
        if (isCorrect == currentQuestionData.isCorrect) 
        {
            Debug.Log("True");
        }
        else
        {
            Debug.Log("False");
        }
        showQuestion();
    }
}
