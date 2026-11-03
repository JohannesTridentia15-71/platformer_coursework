using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsUI : MonoBehaviour
{
    [SerializeField] private Toggle _musicToggle;
    [SerializeField] private Slider _volumeSlider;

    private void Start()
    {
        var sm = SoundManager.Instance;

        // Set UI to current values BEFORE adding listeners
        _musicToggle.SetIsOnWithoutNotify(sm.IsMusicOn());
        _volumeSlider.SetValueWithoutNotify(sm.GetVolume());

        _musicToggle.onValueChanged.AddListener(sm.SetMusicOn);
        _volumeSlider.onValueChanged.AddListener(sm.SetVolume);
    }
}