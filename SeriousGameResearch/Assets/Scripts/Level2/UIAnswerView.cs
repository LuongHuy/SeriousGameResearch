using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIAnswerView : MonoBehaviour
{
    public event Action<int> OnPickAnswerHandle;

    [SerializeField]
    private Color colorNormal;
    [SerializeField]
    private Color colorChange;
    [SerializeField]
    private Image myImg;
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private TextMeshProUGUI answerTxt;
    [SerializeField]
    private Image characterImg;
    [SerializeField]
    private Level2QuestionConfig questionConfig;

    private bool isPicked;
    private int answerIndex;

    public void OnCheckPickFromQuestionView(RectTransform questionTrans)
    {
        isPicked = IsOverlapping(rectTransform, questionTrans);
        myImg.color = isPicked ? colorChange : colorNormal;

        if (isPicked)
        {
            characterImg.sprite = questionConfig.GetCharacterMoodSprite(Level2QuestionConfig.CharacterMood.Exciting);
        }
        else {
            characterImg.sprite = questionConfig.GetCharacterMoodSprite(Level2QuestionConfig.CharacterMood.Idle);
        }
            
    }

    public void OnPickAnswer()
    {
        if (!isPicked)
        {
            return;
        }

        OnPickAnswerHandle?.Invoke(answerIndex);
    }

    public void PickResultForView(Color color, bool isRight)
    {
        myImg.color = color;

        if (isRight) {
            characterImg.sprite = questionConfig.GetCharacterMoodSprite(Level2QuestionConfig.CharacterMood.Fun);
        } else {
            characterImg.sprite = questionConfig.GetCharacterMoodSprite(Level2QuestionConfig.CharacterMood.Sad);
        }
            
    }

    public void ResetValue()
    {
        myImg.color = colorNormal;
        isPicked = false;
        characterImg.sprite = questionConfig.GetCharacterMoodSprite(Level2QuestionConfig.CharacterMood.Idle);
    }

    public void SetAnswert(string answer, int index)
    {
        answerTxt.text = answer;
        answerIndex = index;
    }

    private bool IsOverlapping(RectTransform rect1, RectTransform rect2)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rect1, rect2.position, null) ||
               RectTransformUtility.RectangleContainsScreenPoint(rect2, rect1.position, null);
    }
}
