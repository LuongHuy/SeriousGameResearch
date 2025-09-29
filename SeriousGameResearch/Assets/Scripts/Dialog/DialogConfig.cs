using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogConfig", menuName = "Config/DialogConfig", order = 1)]
public class DialogConfig : ScriptableObject
{
    [SerializeField]
    private CharacterAvatarData[] characterAvatars;

    [SerializeField]
    private ConversationData[] conversations;

    public ConversationData GetConversation(string conversationID)
    {
        foreach (var conversation in conversations)
        {
            if (conversation.conversationID == conversationID)
            {
                return conversation;
            }
        }

        throw new Exception($"Conversation ID {conversationID} not found.");
    }

    public Sprite GetCharacterAvatar(CharacterType characterType, CharacterMood characterMood)
    {
        foreach (var avatarData in characterAvatars)
        {
            if (avatarData.CharacterType == characterType)
            {
                foreach (var moodData in avatarData.MoodSpriteDatas)
                {
                    if (moodData.CharacterMood == characterMood)
                    {
                        return moodData.Sprite;
                    }
                }
            }
        }

        throw new Exception($"Avatar for {characterType} with mood {characterMood} not found.");
    }
}
