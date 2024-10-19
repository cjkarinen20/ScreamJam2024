using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_MuzzleFlashTester : MonoBehaviour
{
    private Scr_MuzzleFlashController m_muzzleFlashController;

    // Start is called before the first frame update
    void Start()
    {
        m_muzzleFlashController = GetComponent<Scr_MuzzleFlashController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            m_muzzleFlashController.PlayMuzzleFlash();
        }
    }
}
