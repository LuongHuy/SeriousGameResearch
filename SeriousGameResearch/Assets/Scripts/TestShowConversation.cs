using UnityEngine;

public class TestShowConversation : MonoBehaviour
{
    [ContextMenu("PlayDialog")]
    private void ShowExampleConversatin() { 
        DialogManager.Instance.ShowConversation("Level_1");
    }
}
