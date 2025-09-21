using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level1Question", menuName = "Config/Level1Question", order = 1)]

public class Level1QuestionConfig : ScriptableObject
{
    [Serializable]
    public struct questionData
    {
        public string question;
        public string answer;
        public bool isCorrect;
    }

    public List<questionData> questionList;
}



