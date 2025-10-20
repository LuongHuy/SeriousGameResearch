using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIQuestionView : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public event Action<RectTransform> OnDragQuestion;
    public event Action OnEndDragQuestion;

    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private TextMeshProUGUI questionTitleTxt;
    [SerializeField]
    private TextMeshProUGUI questionContentTxt;
    [SerializeField]
    private UIDragTutorial uiDragTutorial;

    private bool canMove;

    public void SetInteract(bool isOn)
    {
        canMove = isOn;
        canvasGroup.alpha = isOn ? 1 : 0;
    }

    public void SetQuestion(string questionTitle, string questionContent, bool isFirstQuestion)
    {
        questionTitleTxt.text = questionTitle;
        questionContentTxt.text = questionContent;

        if (isFirstQuestion)
        {
            uiDragTutorial.StartTutorial();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canMove == false)
            return;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(canMove == false) 
            return;

        transform.position = Mouse.current.position.ReadValue();
        OnDragQuestion?.Invoke(rectTransform);
        
        uiDragTutorial.StopTutorial();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(canMove == false) 
            return;

        OnEndDragQuestion?.Invoke();
    }
}
