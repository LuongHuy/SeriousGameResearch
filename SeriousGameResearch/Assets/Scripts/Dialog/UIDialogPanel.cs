using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogPanel : MonoBehaviour
{
    public bool IsShowFullDialog { get; private set; }

    [Header("UI")]
    [SerializeField] private Image characterAvatarImg;
    [SerializeField] private TextMeshProUGUI dialogTxt;

    [SerializeField] private GameObject hintPanel;
    [SerializeField] private TextMeshProUGUI hitTxt;

    [Header("Typing")]
    [SerializeField] private float timeShowCharacter = 0.01f;

    [Header("Type SFX")]
    [SerializeField] private AudioSource sfxSource;     // 2D, Loop=Off, PlayOnAwake=Off
    [SerializeField] private AudioClip beepClip;
    [SerializeField, Range(0f, 1f)] private float beepVolume = 0.7f;
    [SerializeField] private int beepEveryNChars = 2;   // bíp mỗi N ký tự (bỏ khoảng trắng)
    [SerializeField] private float minBeepInterval = 0.02f; // giãn cách tối thiểu giữa 2 bíp (giây)
    [SerializeField] private Vector2 randomPitch = new Vector2(0.96f, 1.04f);
    [SerializeField] private bool enableBeep = true;

    private string currentContent;

    // Counter cho bíp
    private int logicalCharCounter = 0;   // đếm ký tự "hợp lệ" (không tính khoảng trắng)
    private float lastBeepTime = -999f;

    private void Reset()
    {
        // auto-find thuận tiện khi add component
        sfxSource = GetComponent<AudioSource>();
    }

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
        if (hasHint) hitTxt.text = hint;

        characterAvatarImg.sprite = characterAvatarSpr;

        // reset counter beep cho câu mới
        logicalCharCounter = 0;
        lastBeepTime = -999f;

        StartCoroutine(ProcessShowDialogContent(dialogContent));
    }

    public void ForceShowFullDialog()
    {
        StopAllCoroutines();
        IsShowFullDialog = true;
        dialogTxt.text = currentContent;
        // không bíp khi skip hết
    }

    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    private IEnumerator ProcessShowDialogContent(string content)
    {
        dialogTxt.text = string.Empty;
        StringBuilder stringBuilder = new();

        var delayShowCharacter = new WaitForSeconds(timeShowCharacter);

        for (var i = 0; i < content.Length; i++)
        {
            char c = content[i];

            stringBuilder.Append(c);
            dialogTxt.text = stringBuilder.ToString();

            // === Beep logic ===
            TryBeepOnChar(c);

            yield return delayShowCharacter;
        }

        IsShowFullDialog = true;
    }

    private void TryBeepOnChar(char c)
    {
        if (!enableBeep) return;
        if (sfxSource == null || beepClip == null) return;

        // Bỏ qua khoảng trắng & ký tự điều khiển
        if (char.IsWhiteSpace(c)) return;

        logicalCharCounter++;
        if (logicalCharCounter % Mathf.Max(1, beepEveryNChars) != 0) return;

        // Giới hạn tốc độ bíp tối thiểu
        if (Time.unscaledTime - lastBeepTime < minBeepInterval) return;
        lastBeepTime = Time.unscaledTime;

        // Đảm bảo phát 2D
        sfxSource.spatialBlend = 0f;
        sfxSource.pitch = Random.Range(randomPitch.x, randomPitch.y);
        sfxSource.PlayOneShot(beepClip, beepVolume);
    }

    // Tuỳ chọn API công khai để bật/tắt hoặc chỉnh âm lượng từ UI
    public void SetBeepEnabled(bool enabled) => enableBeep = enabled;
    public void SetBeepVolume(float v) => beepVolume = Mathf.Clamp01(v);
}
