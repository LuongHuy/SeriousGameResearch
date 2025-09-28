using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

    [SerializeField]
    private DialogConfig dialogConfig;
    [SerializeField]
    private UIDialogPanel dialogPanel;
    [SerializeField]
    private Button dialogGlobalBtn;
    [SerializeField]
    private float delaySkipShowDialog = 1f;

    private Action endConversationAction;
    private bool isInteractDialog;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        dialogGlobalBtn.onClick.RemoveAllListeners();
        dialogGlobalBtn.onClick.AddListener(() => isInteractDialog = true);

        dialogGlobalBtn.enabled = false;
    }

    public void ShowConversation(string conversationID, Action endConversationAction = null)
    {
        var conversation = dialogConfig.GetConversation(conversationID);
        if (string.IsNullOrEmpty(conversation.conversationID)){
            Debug.Log("Cannot find conversation with ID: " + conversationID);
            return;
        }


        this.endConversationAction = endConversationAction;
        
        Queue<DialogData> dialogQueue = new Queue<DialogData>(conversation.Dialogs);

        StartCoroutine(ProgressDialog(dialogQueue));
    }

    private IEnumerator ProgressDialog(Queue<DialogData> dialogs)
    {
        var delaySkipDialog = new WaitForSeconds(delaySkipShowDialog);

        while (true)
        {
            if (dialogs.Count == 0)
            {
                break;
            }

            var dialog = dialogs.Dequeue();
            dialogPanel.SetActive(true);
            dialogPanel.ShowDialog(dialog.content, dialogConfig.GetCharacterAvatar(dialog.characterType, dialog.characterMood), dialog.hint);

            yield return delaySkipDialog;

            isInteractDialog = false;
            dialogGlobalBtn.enabled = true;

            while (true) {
                if (isInteractDialog)
                {
                    isInteractDialog = false;

                    if (dialogPanel.IsShowFullDialog)
                    {
                        break;
                    }
                    else
                    {
                        dialogPanel.ForceShowFullDialog();
                    }
                }

                yield return null;
            }
        }

        dialogPanel.SetActive(false);
        dialogGlobalBtn.enabled = false;
        endConversationAction?.Invoke();
    }
}
