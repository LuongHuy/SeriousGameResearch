using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogPanel : MonoBehaviour
{
    public bool IsShowFullDialog { get; private set; }

    [SerializeField]
    private Image characterAvatarImg;
    [SerializeField]
    private TextMeshProUGUI dialogTxt;

    [SerializeField]
    private GameObject hintPanel;
    [SerializeField]
    private TextMeshProUGUI hitTxt;

    [SerializeField]
    private float timeShowCharacter = 0.01f;

    private string currentContent;

    private void OnDisable()
    {
        hintPanel.SetActive(false);
    }

    public void ShowDialog(string dialogContent, Sprite characterAvatarSpr, string hint = "")
    {
        IsShowFullDialog = false;

        currentContent = dialogContent;

        bool hasHint = !string.IsNullOrEmpty(hint);
        hintPanel.SetActive(hasHint);

        if (hasHint)
        {
            hitTxt.text = hint;
        }

        characterAvatarImg.sprite = characterAvatarSpr;

        StartCoroutine(ProcessShowDialogContent(dialogContent));
    }

    public void ForceShowFullDialog()
    {
        StopAllCoroutines();
        IsShowFullDialog = true;
        dialogTxt.text = currentContent;
    }

    public void SetActive(bool isActive) { 
        gameObject.SetActive(isActive);
    }

    private IEnumerator ProcessShowDialogContent(string content)
    {
        dialogTxt.text = string.Empty;
        StringBuilder stringBuilder = new ();

        var delayShowCharacter = new WaitForSeconds(timeShowCharacter);

        for (var i =0; i < content.Length; i++)
        {
            stringBuilder.Append(content[i]);
            dialogTxt.text = stringBuilder.ToString();
            yield return delayShowCharacter;
        }

        IsShowFullDialog = true;
    }

}
