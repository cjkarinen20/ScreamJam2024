using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class Scr_MuzzleFlashController : MonoBehaviour
{
    [SerializeField] private GameObject m_muzzleFlashLightPrefab;

    private VisualEffect m_muzzleFlashEffect;
    private GameObject m_cachedLight;

    private float m_muzzleFlashTimer;
    private const float m_muzzleFlashTime = 0.1f;

    private void Start()
    {
        m_muzzleFlashEffect = GetComponentInChildren<VisualEffect>();
    }

    private void Update()
    {
        if(m_muzzleFlashTimer > 0)
        {
            m_muzzleFlashTimer -= Time.deltaTime;
        }
        else if(m_cachedLight != null)
        {
            Destroy(m_cachedLight);
        }
    }

    /// <summary>
    /// Call this method to play the muzzle flash vfx at the attached gameobjet's position
    /// </summary>
    public void PlayMuzzleFlash()
    {
        m_muzzleFlashEffect.Play();

        if(m_cachedLight != null)
        {
            Destroy(m_cachedLight);
        }

        m_cachedLight = Instantiate(m_muzzleFlashLightPrefab, transform);
        m_muzzleFlashTimer = m_muzzleFlashTime;
    }
}
