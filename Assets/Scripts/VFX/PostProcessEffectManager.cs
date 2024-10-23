using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using AustenKinney.Essentials;

public class PostProcessEffectManager : Singleton<PostProcessEffectManager>
{
    [SerializeField] private Volume standardPostProcessingVolume;

    [Header("Tunnel Settings")]
    [SerializeField] private Volume tunnelPostProcessingVolume;
    [SerializeField] private float tunnelFadeTime = 0.5f;

    [Header("Concussion Effect Settings")]
    [SerializeField] private Volume concussionPostProcessingVolume;
    [SerializeField] private float concussionFadeInTime = 0.2f;
    [SerializeField] private float concussionFadeOutTime = 2.5f;

    private TransitionState currentTransitionState = TransitionState.None;
    private VolumeType currentVolume = VolumeType.Standard;
    private float transitionTimer = 0;

    private int currentTunnelVolumeCount = 0;

    private enum TransitionState
    {
        TransitionIn,
        TransitionOut,
        None
    }

    private enum VolumeType
    {
        Standard,
        Tunnel,
        Concussion
    }

    private void Update()
    {
        if (currentVolume == VolumeType.Concussion)
        {
            if (currentTransitionState == TransitionState.TransitionIn)
            {
                transitionTimer += Time.deltaTime;

                float lerpValue = transitionTimer / concussionFadeInTime;

                standardPostProcessingVolume.weight = Mathf.Lerp(1, 0, lerpValue);
                concussionPostProcessingVolume.weight = 1;

                if (transitionTimer >= concussionFadeInTime)
                {
                    transitionTimer = 0;
                    currentTransitionState = TransitionState.TransitionOut;
                    concussionPostProcessingVolume.priority = 1;
                    standardPostProcessingVolume.priority = 0;
                }
            }
            else if (currentTransitionState == TransitionState.TransitionOut)
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
                    currentVolume = VolumeType.Standard;
                }
            }
        }
        else if(currentVolume == VolumeType.Tunnel)
        {
            float lerpValue;

            if (currentTransitionState == TransitionState.TransitionIn)
            {
                if (transitionTimer >= tunnelFadeTime)
                {
                    transitionTimer = tunnelFadeTime;
                }
                else
                {
                    transitionTimer += Time.deltaTime;
                    tunnelPostProcessingVolume.priority = 1;
                    standardPostProcessingVolume.priority = 0;
                }
            }
            else if (currentTransitionState == TransitionState.TransitionOut)
            {

                if (transitionTimer <= 0)
                {
                    transitionTimer = 0;
                    currentTransitionState = TransitionState.None;

                    currentVolume = VolumeType.Standard;
                }
                else
                {
                    transitionTimer -= Time.deltaTime;
                    tunnelPostProcessingVolume.priority = 0;
                    standardPostProcessingVolume.priority = 1;
                }
            }

            lerpValue = transitionTimer / tunnelFadeTime;

            standardPostProcessingVolume.weight = Mathf.Lerp(1, 0, lerpValue);
            tunnelPostProcessingVolume.weight = Mathf.Lerp(0, 1, lerpValue);
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
            currentVolume = VolumeType.Concussion;
        }
    }

    public void EnterTunnel()
    {
        currentTransitionState = TransitionState.TransitionIn;
        currentTunnelVolumeCount += 1;
        currentVolume = VolumeType.Tunnel;
    }

    public void ExitTunnel()
    {
        currentTunnelVolumeCount -= 1;
        if (currentTunnelVolumeCount <= 0)
        {
            currentTransitionState = TransitionState.TransitionOut;
        }
    }
}
