using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTester : MonoBehaviour
{
    PostProcessEffectManager postProcessEffectManager;

    // Start is called before the first frame update
    void Start()
    {
        postProcessEffectManager = PostProcessEffectManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            postProcessEffectManager.TriggerConcussionEffect();
        }
    }
}
