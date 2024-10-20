using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using AustenKinney.Essentials;

public class PostProcessEffectManager : Singleton<PostProcessEffectManager>
{
    [SerializeField] private Volume standardPostProcessingVolume;

    [Header("Concussion Effect Settings")]
    [SerializeField] private Volume concussionPostProcessingVolume;
    [SerializeField] private float concussionFadeInTime = 0.2f;
    [SerializeField] private float concussionFadeOutTime = 2.5f;

    private TransitionState currentTransitionState = TransitionState.None;
    private float transitionTimer = 0;

    private enum TransitionState
    {
        TransitionIn,
        TransitionOut,
        None
    }

    private void Update()
    {

        if(currentTransitionState == TransitionState.TransitionIn)
        {
            transitionTimer += Time.deltaTime;

            float lerpValue = transitionTimer / concussionFadeInTime;

            standardPostProcessingVolume.weight = Mathf.Lerp(1, 0, lerpValue);
            concussionPostProcessingVolume.weight = 1;

            if(transitionTimer >= concussionFadeInTime)
            {
                transitionTimer = 0;
                currentTransitionState = TransitionState.TransitionOut;
                concussionPostProcessingVolume.priority = 1;
                standardPostProcessingVolume.priority = 0;
            }
        }
        else if(currentTransitionState == TransitionState.TransitionOut)
        {
            transitionTimer += Time.deltaTime;

            float lerpValue = transitionTimer / concussionFadeOutTime;

            standardPostProcessingVolume.weight = 1;
            concussionPostProcessingVolume.weight = Mathf.Lerp(1, 0, lerpValue);

            if (transitionTimer >= concussionFadeOutTime)
            {
                transitionTimer = 0;
                currentTransitionState = TransitionState.None;
                concussionPostProcessingVolume.priority = 0;
                standardPostProcessingVolume.priority = 1;
            }
        }
    }

    /// <summary>
    /// Call this method to play the concussion vfx.
    /// </summary>
    public void TriggerConcussionEffect()
    {
        if (currentTransitionState == TransitionState.None)
        {
            currentTransitionState = TransitionState.TransitionIn;
        }
    }
}
