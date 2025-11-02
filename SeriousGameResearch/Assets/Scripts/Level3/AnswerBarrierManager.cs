using System;
using UnityEngine;

public class AnswerBarrierManager : MonoBehaviour
{
    [SerializeField]
    private AnswerBarrierGroup answerBarrierPrefab;

    public void CreateAnswerBarrie(int amount, float spacing, Func<bool, bool> onPlayerAnswer) { 
        for(var i = 1; i <= amount; i++)
        {
            var barrier = Instantiate(answerBarrierPrefab, transform);
            barrier.Init(new Vector3(0, i * spacing, 0), onPlayerAnswer);
        }
    }
}
