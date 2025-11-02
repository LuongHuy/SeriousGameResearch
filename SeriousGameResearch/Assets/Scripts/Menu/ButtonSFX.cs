using UnityEngine;
using UnityEngine.UI;

public class ButtonSFX : MonoBehaviour
{
    public AudioSource sfxSource;
    public AudioClip clickSound;
    public Button button;

    void Start()
    {
        if (button != null)
            button.onClick.AddListener(PlaySFX);
    }

    void PlaySFX()
    {
        if (sfxSource != null && clickSound != null)
            sfxSource.PlayOneShot(clickSound);
    }
}
