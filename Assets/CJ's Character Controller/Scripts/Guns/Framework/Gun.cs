using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunData gunData;
    private NewFPSController playerController;
    private Transform cameraTransform;

    private float currentAmmo = 0f;
    private float nextTimeToFire = 0f;

    private bool isReloading = false;

    private void Start()
    {
        currentAmmo = gunData.magazineSize;

        playerController = transform.root.GetComponent<NewFPSController>();
        cameraTransform = playerController.playerCamera.transform;
    }

    private void OnDisable() {
        StopAllCoroutines();
        CancelInvoke();
        isReloading = false;
    }

    public virtual void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            TryShoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
        }
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

        Debug.Log(gunData.name + ": is reloading....");

        yield return new WaitForSeconds(gunData.reloadTime);

        currentAmmo = gunData.magazineSize;
        isReloading = false;

        Debug.Log(gunData.name + ": is reloaded");
    }
    public void TryShoot()
    {
        if (isReloading){
            Debug.Log(gunData.name + ": is reloading...");
            return;
        }
        if (currentAmmo <= 0f){
            Debug.Log(gunData.name + ": no ammo left, please reload");
            return;
        }

        if (Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + (1 / gunData.fireRate);
            HandleShoot();
        }
    }
    private void HandleShoot()
    {
        currentAmmo--;
        Debug.Log(gunData.name + ": Shot, Ammo Left:" + currentAmmo);
        Shoot();
    }

    public virtual void Shoot(){
        playerController.AddRecoil(gunData);

        RaycastHit hit;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, gunData.shootingRange, gunData.targetLayerMask))
        {
            Debug.Log(gunData.name + ": hit " + hit.collider.name);
        }
    }
}
