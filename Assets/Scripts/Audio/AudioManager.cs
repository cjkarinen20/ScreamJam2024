using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustenKinney.Essentials;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public static float MasterVolume = 1.0f;
    public static float MusicVolume = 0.8f;
    public static float SFXVolume = 0.8f;
    public static float AmbientVolume = 0.8f;

    [SerializeField] private AudioMixer mainAudioMixer;

    [SerializeField] private AudioMixerGroup masterMixerGroup;
    [SerializeField] private AudioMixerGroup musicMixerGroup;
    [SerializeField] private AudioMixerGroup sfxMixerGroup;
    [SerializeField] private AudioMixerGroup ambientMixerGroup;
    [SerializeField] private AudioMixerGroup dialogueMixerGroup;

    private List<AudioSource> audioSourcePool = new List<AudioSource>();

    public delegate void AdjustVolumeLevels();
    public event AdjustVolumeLevels OnAdjustVolumeLevels;

    private void Start()
    {
       
    }

    /// <summary>
    /// Plays a Sound Effect on the Sound Effect Mixer at the given position in worldSpace
    /// </summary>
    /// <param name="position">The position the sound effect will be played in world space</param>
    /// <param name="sfx">The sound effect AudioClip to be played</param>
    public void PlaySFX(Vector3 position, AudioClip sfx, AudioCategory category = AudioCategory.SFX)
    {
        AudioSource audioSource = GetAudioSource();
        audioSource.transform.position = position;
        audioSource.clip = sfx;
        audioSource.outputAudioMixerGroup = GetMixerGroup(category);
        audioSource.spatialBlend = 1;
        audioSource.pitch = 1;
        audioSource.PlayOneShot(sfx);
    }

    /// <summary>
    /// Plays a Sound Effect on the Sound Effect Mixer in 2D space.
    /// </summary>
    /// <param name="sfx"></param>
    public void PlaySFX(AudioClip sfx, AudioCategory category = AudioCategory.SFX)
    {
        AudioSource audioSource = GetAudioSource();
        audioSource.clip = sfx;
        audioSource.outputAudioMixerGroup = GetMixerGroup(category);
        audioSource.spatialBlend = 0;
        audioSource.pitch = 1;
        audioSource.PlayOneShot(sfx);
    }

    /// <summary>
    /// Plays a Sound Effect randomly pitched up or down slightly at the given position in world space. Use this to add variety to repetitive sounds (ie. footsteps).
    /// </summary>
    /// <param name="position">The position the sound effect will be played in world space</param>
    /// <param name="sfx">The sound effect AudioClip to be played</param>
    public void PlaySFXRandomPitch(Vector3 position, AudioClip sfx, AudioCategory category = AudioCategory.SFX)
    {
        AudioSource audioSource = GetAudioSource();
        audioSource.transform.position = position;
        audioSource.clip = sfx;
        audioSource.outputAudioMixerGroup = GetMixerGroup(category);
        float pitch = Random.Range(0.9f, 1.1f);
        audioSource.spatialBlend = 1;
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(sfx);
    }

    /// <summary>
    /// Plays a Sound Effect randomly pitched up or down slightly. Use this to add variety to repetitive sounds (ie. footsteps).
    /// </summary>
    /// <param name="sfx">The sound effect AudioClip to be played</param>
    public void PlaySFXRandomPitch(AudioClip sfx, AudioCategory category = AudioCategory.SFX)
    {
        AudioSource audioSource = GetAudioSource();
        audioSource.clip = sfx;
        audioSource.outputAudioMixerGroup = GetMixerGroup(category);
        float pitch = Random.Range(0.9f, 1.1f);
        audioSource.spatialBlend = 0;
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(sfx);
    }

    private AudioSource GetAudioSource()
    {
        for (int i = 0; i < audioSourcePool.Count; i++)
        {
            if(audioSourcePool[i].isVirtual == true)
            {
                return audioSourcePool[i];
            }
        }

        GameObject newGameObject = new GameObject("SFX Audio Source");
        DontDestroyOnLoad(newGameObject);
        AudioSource newAudioSource = newGameObject.AddComponent<AudioSource>();

        audioSourcePool.Add(newAudioSource);
        return newAudioSource;
    }


    public void SetMasterVolume(float volume)
    {
        MasterVolume = volume;
        mainAudioMixer.SetFloat("masterVolume", volume);

        if(OnAdjustVolumeLevels != null)
        {
            OnAdjustVolumeLevels.Invoke();
        }
    }

    public void SetMusicVolue(float volume)
    {
        MusicVolume = volume;
        mainAudioMixer.SetFloat("musicVolume", volume);

        if(OnAdjustVolumeLevels != null)
        {
            OnAdjustVolumeLevels.Invoke();
        }
    }

    public void SetSFXVolume(float volume)
    {
        SFXVolume = volume;
        mainAudioMixer.SetFloat("sfxVolume", volume);

        if (OnAdjustVolumeLevels != null)
        {
            OnAdjustVolumeLevels.Invoke();
        }
    }

    public void SetAmbientVolume(float volume)
    {
        AmbientVolume = volume;
        mainAudioMixer.SetFloat("ambientVolume", volume);

        if (OnAdjustVolumeLevels != null)
        {
            OnAdjustVolumeLevels.Invoke();
        }
    }

    public void SetDialogueVolume(float volume)
    {
        AmbientVolume = volume;
        mainAudioMixer.SetFloat("dialogueVolume", volume);

        if (OnAdjustVolumeLevels != null)
        {
            OnAdjustVolumeLevels.Invoke();
        }
    }

    private AudioMixerGroup GetMixerGroup(AudioCategory category)
    {
        switch (category)
        {
            case AudioCategory.Master:
                return masterMixerGroup;
            case AudioCategory.Music:
                return musicMixerGroup;
            case AudioCategory.Ambient:
                return ambientMixerGroup;
            case AudioCategory.SFX:
                return sfxMixerGroup;
            case AudioCategory.Dialogue:
                return dialogueMixerGroup;
            default:
                return masterMixerGroup;
        }
    }
}
