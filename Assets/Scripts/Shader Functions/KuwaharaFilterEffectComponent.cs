using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenuForRenderPipeline("Post-processing/Kuwahara Filter", typeof(UniversalRenderPipeline))]
public class KuwaharaFilterEffectComponent : VolumeComponent, IPostProcessComponent
{
    public ClampedIntParameter intensity = new ClampedIntParameter(0, 0, 50, false);


    public bool IsActive()
    {
        return true;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}
