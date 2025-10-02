using System;
using UnityEngine;

public class StreetManager : MonoBehaviour
{
    [Header("Street Line")]
    [SerializeField]
    private Transform linePrefab;
    [SerializeField]
    private Transform lineContainer;
    [SerializeField]
    private int totalLine;
    [SerializeField]
    private int lineSpacing;

    [Header("Barrier")]
    [SerializeField]
    private AnswerBarrierManager answerBarrierManager;

    public void InitStreet(int totalQuestion, float spacing, Func<bool, bool> answerAction) { 
        answerBarrierManager.CreateAnswerBarrie(totalQuestion, spacing, answerAction);
    }

    [ContextMenu("Create Line")]
    private void CreateLine()
    {
        RemoveAllCurrentLine();
        CreateLines();
    }

    private void CreateLines()
    {
        for(var i = 0; i < totalLine; i++)
        {
            var line = Instantiate(linePrefab, lineContainer);
            line.localPosition = new Vector3(0, i * lineSpacing, 0);
        }
    }

    private void RemoveAllCurrentLine() { 
        var childCount = lineContainer.childCount;
        if(childCount < 0)
        {
            return;
        }

        for(var i = childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(lineContainer.GetChild(i).gameObject);
        }
    }
}
