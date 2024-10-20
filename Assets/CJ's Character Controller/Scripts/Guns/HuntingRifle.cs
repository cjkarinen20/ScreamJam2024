using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HuntingRifle : Gun
{
    [SerializeField] private Animator anim;
    [SerializeField] private NewFPSController fpsController;
    [SerializeField] private TextMeshProUGUI ammoText;

    private void OnEnable() {
        anim.Rebind();
        ammoText.text = currentAmmo.ToString("0") + "/" + gunData.magazineSize;
    }
    public override void Update()
    {
        base.Update();

        ammoText.text = currentAmmo.ToString("0") + "/" + gunData.magazineSize;

        switch (fpsController.playerMovementState) {
            case NewFPSController.PlayerMovementState.IDLE:
                anim.ResetTrigger("Walk");
                anim.ResetTrigger("Run");
                anim.SetTrigger("Idle");
                break;
            case NewFPSController.PlayerMovementState.WALKING:
                anim.ResetTrigger("Idle");
                anim.ResetTrigger("Run");
                anim.SetTrigger("Walk");
                break;
            case NewFPSController.PlayerMovementState.RUNNING:
                anim.ResetTrigger("Walk");
                anim.ResetTrigger("Idle");
                anim.SetTrigger("Run");
                break;
            default :
                
                break;
        }
    }

    public override void TryShoot()
    {
        if (isReloading){
            Debug.Log(gunData.name + ": is reloading...");
            ReloadEnd();
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

    public override void Shoot(){
        base.Shoot();

        anim.Play("RemingtonShoot");
    }

    public override void TryReload()
    {
        if (!isReloading && currentAmmo < gunData.magazineSize)
        {
            ReloadBegin();
        }
    } 

    private void ReloadBegin () {
        isReloading = true;
        anim.SetTrigger("ReloadBegin");
        Invoke("ReloadMidway", .9f);
    }
    private void ReloadMidway () {
        if(currentAmmo < gunData.magazineSize){
            anim.SetTrigger("ReloadMidway");
            Invoke("AddBullet", .45f);
        }else{
            ReloadEnd();
        }
        
    }
    private void AddBullet(){
        currentAmmo++;
        if(currentAmmo < gunData.magazineSize){
            Invoke("ReloadMidway", .55f);
        }else{
            ReloadEnd();
        }
    }
    private void ReloadEnd () {
        CancelInvoke("AddBullet");
        CancelInvoke("ReloadMidway");

        anim.ResetTrigger("ReloadBegin");
        anim.ResetTrigger("ReloadMidway");

        Invoke("StopReload", .833f);

        anim.SetTrigger("ReloadEnd");
    }

    private void StopReload () {
        isReloading = false;
    }
}
