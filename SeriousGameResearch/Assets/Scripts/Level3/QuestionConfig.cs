using System;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionConfig", menuName = "Config/QuestionConfig_Level3", order = 1)]
public class QuestionConfig : ScriptableObject
{
    [Serializable]
    public class QuestionData
    {
        public int id;
        public string question;
        public bool answer;
    }

    public QuestionData[] Questions => questions;
    public float TimePerQuestion = 3.5f;

    [SerializeField]
    private QuestionData[] questions;
}
