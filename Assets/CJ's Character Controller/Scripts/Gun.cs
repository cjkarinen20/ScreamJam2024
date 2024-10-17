using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunData gunData;
    public NewFPSController playerController;
    public Transform cameraTransform;
    public Camera camera;

    private float currentAmmo = 0f;
    private float nextTimeToFire = 0f;

    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = gunData.magazineSize;

        playerController = transform.root.GetComponent<NewFPSController>();
        cameraTransform = camera.transform;
    }
    public void TryReload()
    {
        if (!isReloading && currentAmmo < gunData.magazineSize)
        {
            StartCoroutine(Reload());
        }
    }    
    private IEnumerator Reload()
    {
        isReloading = true;

        Debug.Log(gunData.gunName + "is reloading....");

        yield return new WaitForSeconds(gunData.reloadTime);

        currentAmmo = gunData.magazineSize;
        isReloading = false;

        Debug.Log(gunData.gunName + "is reloaded");
    }
}
