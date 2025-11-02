using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private StreetManager streetManager;
    [SerializeField]
    private QuestionUIView questionUIView;
    [SerializeField]
    private QuestionConfig questionConfig;

    private Queue<QuestionConfig.QuestionData> questionQueue;
    private QuestionConfig.QuestionData currentQuestion;
    private float delayAnswer;

    public GameObject winGameUI;
    public GameObject loseGameUI;

    public GameObject trueText;
    public GameObject falseText;

    private int curHealth;
    public TextMeshProUGUI healthText;

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
    }

    private void Start() {
        curHealth = 3;
        healthText.text = curHealth.ToString();
        DialogManager.Instance.ShowConversation("Level_3", OnConversationEnd);
        questionQueue = new Queue<QuestionConfig.QuestionData>(questionConfig.Questions);
        streetManager.InitStreet(questionConfig.Questions.Length, characterController.ForwardSpeed * questionConfig.TimePerQuestion, AnswerQuestion);
    }

    public void GenerateQuestion() { 
        if(questionQueue.Count == 0)
        {          
            ShowGameoverWin();
            return;
        }

        currentQuestion = questionQueue.Dequeue();
        questionUIView.ShowQuestion(currentQuestion.question);
    }

    public bool AnswerQuestion(bool isTrue) { 
        if(currentQuestion == null || delayAnswer > Time.time)
        {
            Debug.LogWarning("No current question to answer.");
            return false;
        }

        if(currentQuestion.answer == isTrue)
        {
          
           StartCoroutine(ShowTextForSeconds(trueText, 0.75f));
        }
        else
        {          
            StartCoroutine(ShowTextForSeconds(falseText, 0.75f));
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

    private void ShowGameoverWin() { 
        characterController.CanMove = false;
        winGameUI.SetActive(true);
    }

    private void ShowGameoverLose()
    {
        characterController.CanMove = false;
        loseGameUI.SetActive(true);

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
