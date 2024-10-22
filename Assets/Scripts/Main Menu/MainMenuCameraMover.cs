using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuCameraMover : MonoBehaviour
{
    [SerializeField] private Transform[] startingPositions;
    [SerializeField] private Transform[] endingPositions;
    [SerializeField] private Image BlackoutPanel;
    [SerializeField] private float trackingSpeed;
    [SerializeField] private float fadeTime;

    private int currentIndex;
    private float transitionTimer;
    private float fadeTimer;
    private bool nextTrack;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transitionTimer += Time.deltaTime;

        float trackingTimer = trackingSpeed * transitionTimer / Vector3.Distance(startingPositions[currentIndex].position, endingPositions[currentIndex].position);

        if (nextTrack == true)
        {
            fadeTimer -= Time.deltaTime;

            BlackoutPanel.color = Color.Lerp(Color.clear, Color.black, fadeTimer / fadeTime);

            if (fadeTimer <= 0)
            {
                fadeTimer = 0;
                nextTrack = false;
            }
        }
        else if (trackingSpeed * (transitionTimer + fadeTime) / Vector3.Distance(startingPositions[currentIndex].position, endingPositions[currentIndex].position) >= 1)
        {
            fadeTimer += Time.deltaTime;
            BlackoutPanel.color = Color.Lerp(Color.clear, Color.black, fadeTimer / fadeTime);

            if (fadeTimer / fadeTime >= 1)
            {
                fadeTimer = 1;
                nextTrack = true;
                transitionTimer = 0;

                currentIndex += 1;
                if (currentIndex >= startingPositions.Length)
                {
                    currentIndex = 0;
                }
            }
        }

        transform.position = Vector3.Lerp(startingPositions[currentIndex].position, endingPositions[currentIndex].position, trackingTimer);
        transform.rotation = Quaternion.Lerp(startingPositions[currentIndex].rotation, endingPositions[currentIndex].rotation, trackingTimer);

    }
}
