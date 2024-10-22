using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] sounds;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomAudioClip () {
        audioSource.clip = sounds[Random.Range(0, sounds.Length)];
        audioSource.Play();
    }
}
