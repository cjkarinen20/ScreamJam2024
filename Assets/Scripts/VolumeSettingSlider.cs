using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettingSlider : MonoBehaviour
{
    [SerializeField] AudioCategory audioCategory;
    [SerializeField] private AudioClip sliderChangedSFX;

    private AudioManager audioManager;
    private Slider slider;

    private const float tickTime = 0.1f;
    private float tickTimer;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.Instance;
        slider = GetComponent<Slider>();

        SetSliderToVolume();
        slider.onValueChanged.AddListener(SetVolume);
    }

    private void Update()
    {
        if(tickTimer > 0)
        {
            tickTimer -= Time.unscaledDeltaTime;
        }
    }

    private void SetVolume(float volume)
    {
        switch (audioCategory)
        {
            case AudioCategory.Master:
                audioManager.SetMasterVolume(volume);
                break;
            case AudioCategory.Music:
                audioManager.SetMusicVolue(volume);
                break;
            case AudioCategory.Ambient:
                audioManager.SetAmbientVolume(volume);
                break;
            case AudioCategory.SFX:
                audioManager.SetSFXVolume(volume);
                break;
            case AudioCategory.Dialogue:
                audioManager.SetDialogueVolume(volume);
                break;
        }

        if (tickTimer <= 0)
        {
            audioManager.PlaySFX(sliderChangedSFX, audioCategory);
            tickTimer = tickTime;
        }
    }

    private void SetSliderToVolume()
    {
        switch (audioCategory)
        {
            case AudioCategory.Master:
                slider.value = AudioManager.MasterVolume;
                break;
            case AudioCategory.Music:
                slider.value = AudioManager.MusicVolume;
                break;
            case AudioCategory.SFX:
                slider.value = AudioManager.SFXVolume;
                break;
            case AudioCategory.Ambient:
                slider.value = AudioManager.AmbientVolume;
                break;
        }
    }

}

public enum AudioCategory
{
    Master,
    Music,
    Ambient,
    SFX,
    Dialogue
}
