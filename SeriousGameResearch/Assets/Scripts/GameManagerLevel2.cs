using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManagerLevel2 : MonoBehaviour
{
    private List<Level2QuestionConfig.questionData> totalQuestion;
    public Level2QuestionConfig questionConfig;

    public float delayShowAnswerResult = 1f;
    public GameObject endGameUI;

    public GameObject panelDialog;

    public UIQuestionView questionView;
    public UIAnswerView[] uIAnswerViews;

    private Level2QuestionConfig.questionData currentQuestionData;
    private int questionPass;

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

        foreach (var uIAnswerView in uIAnswerViews)
        {
            questionView.OnDragQuestion += uIAnswerView.OnCheckPickFromQuestionView;
            questionView.OnEndDragQuestion += uIAnswerView.OnPickAnswer;
            
            uIAnswerView.OnPickAnswerHandle += onClickToAnswer;
        }

        questionView.SetInteract(true);
    }

    private void ResetAnswersAndQuestion() {
        questionView.transform.localPosition = Vector3.zero;
        questionView.SetInteract(false);

        foreach (var uIAnswerView in uIAnswerViews)
        {
            uIAnswerView.ResetValue();
        }
    }

    private void showQuestion()
    {
        ResetAnswersAndQuestion();
        if (totalQuestion.Count == 0)
        {
            Debug.Log("EndGame");
            

            if (endGameUI != null)
            {
                endGameUI.SetActive(true);
            }
            return;
        }
        var questionIndex = Random.Range(0, totalQuestion.Count);
        currentQuestionData = totalQuestion[questionIndex];
        totalQuestion.RemoveAt(questionIndex);

        for(var i = 0; i < currentQuestionData.result.Length; i++)
        {
            if(i < uIAnswerViews.Length)
            {
                uIAnswerViews[i].SetAnswert(currentQuestionData.result[i], i);
            }
        }

        questionView.SetQuestion(currentQuestionData.question, currentQuestionData.answer, questionPass == 0);
        questionView.SetInteract(true);

        questionPass++;
    }

    public async void onClickToAnswer(int correctIndex)
    {
        questionView.SetInteract(false);
        var answerView = uIAnswerViews[correctIndex];

        if (correctIndex == currentQuestionData.correctIndex)
        {
            Debug.Log("True");
            answerView.PickResultForView(Color.green, true);

        }
        else
        {
            Debug.Log("False");
            answerView.PickResultForView(Color.red, false);

            foreach (var view in uIAnswerViews)
            {
                if (view.AnswerIndex != currentQuestionData.correctIndex)
                {
                    continue;
                }
                
                view.PickResultForView(Color.green, false);
                break;
            }
            
        }

        await Task.Delay((int)(delayShowAnswerResult * 1000));

        showQuestion();
    }
}
