using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelVolumeController : MonoBehaviour
{
    private PostProcessEffectManager postProcessEffectManager;

    // Start is called before the first frame update
    void Start()
    {
        postProcessEffectManager = PostProcessEffectManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            postProcessEffectManager.EnterTunnel();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            postProcessEffectManager.ExitTunnel();
        }
    }
}
