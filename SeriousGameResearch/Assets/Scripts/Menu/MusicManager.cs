using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Sources (both Loop, no PlayOnAwake)")]
    public AudioSource sourceA;
    public AudioSource sourceB;

    [Header("Clips")]
    public AudioClip menuBGM;      // dùng cho Main Menu + Level Selection
    public AudioClip gameplayBGM;  // dùng cho Level1 + Level2 + Level3

    [Header("Scene Groups (by name)")]
    public string[] menuScenes = new string[] { "Main Menu", "Level Selection" };
    public string[] gameplayScenes = new string[] { "Level1", "Level2", "Level3" };

    [Header("Mix")]
    [Range(0f, 1f)] public float defaultVolume = 0.6f;
    [Range(0.1f, 5f)] public float fadeTime = 1.5f;

    private static MusicManager instance;
    private AudioSource active, idle;

    void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);

        sourceA.loop = sourceB.loop = true;
        sourceA.playOnAwake = sourceB.playOnAwake = false;

        active = sourceA;
        idle = sourceB;
    }

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void Start()
    {
        // đảm bảo phát đúng nhạc nếu game khởi động ở giữa
        ApplyMusicFor(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyMusicFor(scene.name);
    }

    void ApplyMusicFor(string sceneName)
    {
        AudioClip next = ResolveClipForScene(sceneName);

        // nếu clip mong muốn đã là clip đang phát → thôi
        if (next == null) return;

        if (active.clip == next)
        {
            if (!active.isPlaying)
            {
                active.volume = defaultVolume;
                active.Play();
            }
            return;
        }

        // crossfade sang clip mới
        StartCoroutine(CrossfadeTo(next));
    }

    AudioClip ResolveClipForScene(string sceneName)
    {
        // Kiểm tra thuộc nhóm nào
        if (menuScenes != null && menuScenes.Contains(sceneName))
            return menuBGM;

        if (gameplayScenes != null && gameplayScenes.Contains(sceneName))
            return gameplayBGM;

        // fallback: nếu scene không nằm trong danh sách, giữ nguyên đang phát
        return active != null ? active.clip : null;
    }

    IEnumerator CrossfadeTo(AudioClip next)
    {
        idle.clip = next;
        idle.volume = 0f;
        idle.Play();

        float t = 0f;
        float startVol = active.isPlaying ? active.volume : defaultVolume;

        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / fadeTime);

            idle.volume = Mathf.Lerp(0f, defaultVolume, k);
            if (active.isPlaying)
                active.volume = Mathf.Lerp(startVol, 0f, k);

            yield return null;
        }

        if (active.isPlaying) active.Stop();
        idle.volume = defaultVolume;

        // hoán đổi vai trò
        var tmp = active; active = idle; idle = tmp;
    }

    // Gắn UI Slider (OnValueChanged) → MusicManager.SetMasterVolume
    public void SetMasterVolume(float v)
    {
        defaultVolume = Mathf.Clamp01(v);
        if (active != null) active.volume = defaultVolume;
    }

    // Tắt/mở nhanh (ví dụ trong Pause Menu)
    public void Mute(bool mute)
    {
        if (active != null) active.mute = mute;
        if (idle != null) idle.mute = mute;
    }
}
