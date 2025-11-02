using System;
using UnityEngine;

public class AnswerBarrierItem : MonoBehaviour
{
    public bool Answer;
    public Action OnFinishAnswer;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private Func<bool, bool> answerAction;

    private void Start()
    {
        spriteRenderer.color = Color.white;
    }

    public void ToggleInteract(bool canInteract) { 
        var collider = GetComponent<Collider2D>();
        if(collider != null)
        {
            collider.enabled = canInteract;
        }
    }

    public void Init(Func<bool, bool> answerAction) { 
        this.answerAction = answerAction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && answerAction != null)
        {
            spriteRenderer.color = answerAction(Answer) ? Color.green : Color.red;

            OnFinishAnswer?.Invoke();
        }
    }
}
