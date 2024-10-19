using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenuForRenderPipeline("Post-processing/Screen Warp", typeof(UniversalRenderPipeline))]
public class ScreenWarpEffectComponent : VolumeComponent, IPostProcessComponent
{
    [Header("Screen Warp Settings")]

    public ClampedFloatParameter intensity = new ClampedFloatParameter(0, 0, 0.2f, false);
    public ClampedFloatParameter scale = new ClampedFloatParameter(1, 0, 5, false);
    public FloatParameter speed = new FloatParameter(1, false);

    public bool IsActive()
    {
        return true;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}
