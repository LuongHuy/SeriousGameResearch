using System;
using System.Collections;
using UnityEngine;

public class UIDragTutorial : MonoBehaviour
{
    [SerializeField] private Transform tutorialObj;
    [SerializeField] private Transform startMovePoint;
    [SerializeField] private Transform endMovePoint;
    
    [Header("Tutorial Stats")]
    [SerializeField] private float moveSpeed = 120;
    [SerializeField] private float timeDelayStartTutorial = 1f;
    [SerializeField] private float timeDelayToLoopTutorial = 0.5f;

    private void Start()
    {
        tutorialObj.gameObject.SetActive(false);
    }

    public void StartTutorial()
    {
        StartCoroutine((MoveTutorial()));
    }

    public void StopTutorial()
    {
        StopAllCoroutines();
        tutorialObj.gameObject.SetActive(false);
    }

    private IEnumerator MoveTutorial()
    {
        yield return new WaitForSeconds(timeDelayStartTutorial);
        
        tutorialObj.gameObject.SetActive(true);
        tutorialObj.position = startMovePoint.position;
        while (true)
        {
            tutorialObj.position = Vector2.MoveTowards(tutorialObj.position, endMovePoint.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(tutorialObj.position, endMovePoint.position) < 0.1f)
            {
                yield return new WaitForSeconds(timeDelayToLoopTutorial);
                
                tutorialObj.position = startMovePoint.position;
            }

            yield return null;
        }
    }
}
