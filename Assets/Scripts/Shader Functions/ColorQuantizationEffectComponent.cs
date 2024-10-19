using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenuForRenderPipeline("Post-processing/ColorQuantization", typeof(UniversalRenderPipeline))]
public class ColorQuantizationEffectComponent : VolumeComponent, IPostProcessComponent
{
    public IntParameter colorDepth = new IntParameter(256, false);

    public bool IsActive()
    {
        return true;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}
