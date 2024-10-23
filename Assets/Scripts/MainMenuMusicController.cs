using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusicController : MonoBehaviour
{
    private const float loopTime = 15f;
    private float loopTimer;

    private AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        musicSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if(musicSource.isPlaying == false)
        {
            loopTimer += Time.deltaTime;

            if(loopTimer >= loopTime)
            {
                musicSource.Play();
                loopTimer = 0;
            }
        }
    }
}
