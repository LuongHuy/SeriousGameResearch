using TMPro;
using UnityEngine;

public class QuestionUIView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI questionTxt;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowQuestion(string question)
    {
        bool canShow = !string.IsNullOrEmpty(question);

        questionTxt.text = question;
        gameObject.SetActive(canShow);
    }
}
