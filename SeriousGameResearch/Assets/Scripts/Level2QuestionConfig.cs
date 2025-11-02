using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level2Question", menuName = "Config/Level2Question", order = 1)]
public class Level2QuestionConfig : ScriptableObject
{
    public enum CharacterMood
    {
        Idle,
        Exciting,
        Fun,
        Sad,
    }

    [Serializable]
    public class CharacterMoodData
    {
        public CharacterMood characterMood;
        public Sprite characterSprite;
    }

    [Serializable]
    public struct questionData
    {
        public string question;
        public string answer;
        public string[] result;
        public int correctIndex;
        
    }

    public List<questionData> questionList;

    [SerializeField]
    private List<CharacterMoodData> characterMoodDatas;

    public Sprite GetCharacterMoodSprite(CharacterMood characterMood)
    {
        if (characterMoodDatas == null || characterMoodDatas.Count == 0)
        {
            Debug.LogWarning("Character mood data is not set.");
            return null;
        }

        var moodData = characterMoodDatas.Find(data => data.characterMood == characterMood);
        if (moodData != null)
        {
            return moodData.characterSprite;
        }

        Debug.LogWarning($"Character mood '{characterMood}' not found.");
        return null;
    }
}
