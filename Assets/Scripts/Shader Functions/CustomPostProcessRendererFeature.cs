using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental;

public class CustomPostProcessRendererFeature : ScriptableRendererFeature
{

    [SerializeField] Settings settings;

    private ScreenWarpPass m_customPass;



    public class ScreenWarpPass : ScriptableRenderPass
    {
        private Settings settings;


        private RTHandle colorTarget;
        private RTHandle depthTarget;
        private RenderTextureDescriptor descriptor;

        private Material screenWarpMaterial;
        private ScreenWarpEffectComponent screenWarpEffect;
        private RTHandle screenWarpTarget;
        private int screenWarpTargetID;

        private Material kuwaharaFilterMaterial;
        private KuwaharaFilterEffectComponent kuwaharaFilterEffect;
        private RTHandle kuwaharaFilterTarget;
        private int kuwaharaFilterTargetID;

        public ScreenWarpPass(Settings s)
        {
            settings = s;

            screenWarpMaterial = CoreUtils.CreateEngineMaterial(settings.screenWarpShader);
            screenWarpTargetID = Shader.PropertyToID("_ScreenWarp");
            screenWarpTarget = RTHandles.Alloc(screenWarpTargetID, name: "_ScreenWarp");

            kuwaharaFilterMaterial = CoreUtils.CreateEngineMaterial(settings.kuwaharaFilterShader);
            kuwaharaFilterTargetID = Shader.PropertyToID("_KuwarahaFilter");
            kuwaharaFilterTarget = RTHandles.Alloc(kuwaharaFilterTargetID, name: "_KuwaharaFilter");

            renderPassEvent = settings.injectionPoint;
        }

        public void Setup(RTHandle cameraColorTarget, RTHandle cameraDepthTarget)
        {
            this.colorTarget = cameraColorTarget;
            this.depthTarget = cameraDepthTarget;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            descriptor = renderingData.cameraData.cameraTargetDescriptor;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            VolumeStack stack = VolumeManager.instance.stack;
            screenWarpEffect = stack.GetComponent<ScreenWarpEffectComponent>();
            kuwaharaFilterEffect = stack.GetComponent<KuwaharaFilterEffectComponent>();

            CommandBuffer cmd = CommandBufferPool.Get();

            ProfilingScope customProfilingScope = new ProfilingScope(cmd, new ProfilingSampler("Custom Post Process Effects"));

            using (customProfilingScope)
            {
                SetupScreenWarp(cmd, colorTarget);

                SetupKuwaharaFilter(cmd, colorTarget);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        private void SetupScreenWarp(CommandBuffer cmd, RTHandle source)
        {
            Blitter.BlitCameraTexture(cmd, source, screenWarpTarget, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare, screenWarpMaterial, 0);

            cmd.SetGlobalFloat("_ScreenWarpIntensity", screenWarpEffect.intensity.value);
            cmd.SetGlobalFloat("_ScreenWarpSpeed", screenWarpEffect.speed.value);
            cmd.SetGlobalFloat("_ScreenWarpScale", screenWarpEffect.scale.value);
        }

        private void SetupKuwaharaFilter(CommandBuffer cmd, RTHandle source)
        {
            Blitter.BlitCameraTexture(cmd, source, kuwaharaFilterTarget, RenderBufferLoadAction.DontCare, RenderBufferStoreAction.DontCare, kuwaharaFilterMaterial, 0);

            cmd.SetGlobalFloat("_KuwaharaIntensity", kuwaharaFilterEffect.intensity.value);
        }
    }


    public override void Create()
    {
        m_customPass = new ScreenWarpPass(settings);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        m_customPass.ConfigureInput(ScriptableRenderPassInput.Color);
        m_customPass.ConfigureInput(ScriptableRenderPassInput.Depth);
        m_customPass.Setup(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_customPass);
    }

    [System.Serializable]
    public class Settings
    {
        //public string TextureName = "_GrabPassTransparent";
        public RenderPassEvent injectionPoint = RenderPassEvent.BeforeRenderingPostProcessing;

        public Shader screenWarpShader;
        public Shader kuwaharaFilterShader;
    }

}
