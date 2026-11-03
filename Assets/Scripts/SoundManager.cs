using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioSource _musicSource;

    private const string VolumeKey = "MusicVolume";
    private const string MuteKey = "MusicOn";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        AudioListener.pause = false;
        // Apply saved settings on startup
        SetVolume(PlayerPrefs.GetFloat(VolumeKey, 0.75f));
        SetMusicOn(PlayerPrefs.GetInt(MuteKey, 1) == 1);

        if (!_musicSource.isPlaying)
            _musicSource.Play();

        Debug.Log($"enabled: {_musicSource.enabled}, activeInHierarchy: {_musicSource.gameObject.activeInHierarchy}, " +
          $"paused(listener): {AudioListener.pause}, listenerVolume: {AudioListener.volume}, " +
          $"clipLoadState: {_musicSource.clip.loadState}, clipLength: {_musicSource.clip.length}, " +
          $"isPlaying: {_musicSource.isPlaying}");
    }

    public float GetVolume() => PlayerPrefs.GetFloat(VolumeKey, 0.75f);
    public bool IsMusicOn() => PlayerPrefs.GetInt(MuteKey, 1) == 1;

    public void SetVolume(float value)
    {
        PlayerPrefs.SetFloat(VolumeKey, value);
        float db = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        _audioMixer.SetFloat("MusicVolume", db);
    }

    public void SetMusicOn(bool isOn)
    {
        PlayerPrefs.SetInt(MuteKey, isOn ? 1 : 0);
        _musicSource.mute = !isOn;
    }
}