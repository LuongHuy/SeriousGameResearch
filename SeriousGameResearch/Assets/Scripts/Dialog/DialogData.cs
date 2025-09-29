using System;
using UnityEngine;

[Serializable]
public struct ConversationData {
    public string conversationID;
    public DialogData[] Dialogs;
}

public enum CharacterType
{
    NCP,
    Player,
}

public enum CharacterMood
{
    Mood_1,
    Mood_2,
    Mood_3,
}

[Serializable]
public struct DialogData
{
    public CharacterType characterType;
    public CharacterMood characterMood;
    public string content;
    public string hint;
}

[Serializable]
public class CharacterAvatarData {
    public CharacterType CharacterType;
    public MoodSpriteData[] MoodSpriteDatas;

    [Serializable]
    public class MoodSpriteData { 
        public CharacterMood CharacterMood;
        public Sprite Sprite;
    }
}

