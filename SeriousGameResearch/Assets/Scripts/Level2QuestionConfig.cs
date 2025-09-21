using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level2Question", menuName = "Config/Level2Question", order = 1)]
public class Level2QuestionConfig : ScriptableObject
{
    [Serializable]
    public struct questionData
    {
        public string question;
        public string answer;
        public string[] result;
        public int correctIndex;
        
    }

    public List<questionData> questionList;
}
