using UnityEngine;

public class AnswerBarrierGroup : MonoBehaviour
{
    [SerializeField]
    private AnswerBarrierItem[] answerBarrierItems;

    public void Init(Vector3 localPos ,System.Func<bool, bool> onPlayerAnswer)
    {
        transform.localPosition = localPos;

        for (var i = 0; i < answerBarrierItems.Length; i++)
        {
            answerBarrierItems[i].Init(onPlayerAnswer);
            answerBarrierItems[i].OnFinishAnswer = DisableBarrierItem;
        }
    }

    private void DisableBarrierItem() { 
        for (var i = 0; i < answerBarrierItems.Length; i++)
        {
            answerBarrierItems[i].ToggleInteract(false);
        }
    }
}
