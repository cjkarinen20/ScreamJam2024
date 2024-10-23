using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HuntingRifle : Gun
{
    [SerializeField] private Animator anim;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private AudioSource shootSound, gunSounds;
    [SerializeField] private AudioClip[] shootSounds;
    [SerializeField] private AudioClip gunFire, reloadBegin, reloadMidway, reloadEnd;
    private bool hasReloadEnded = false;

    private void OnEnable() {
        hasReloadEnded = false;

        currentAmmo = gunData.magazineSize;

        CancelInvoke();
        StopAllCoroutines();

        ResetMuzzleFlash();

        anim.Rebind();
        
        ammoText.text = currentAmmo.ToString("0") + "/" + gunData.magazineSize;
    }
    public override void Update()
    {
        base.Update();

        ammoText.text = currentAmmo.ToString("0") + "/" + gunData.magazineSize;

        switch (playerController.playerMovementState) {
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
        if(!playerController.canMove) return;

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

        gunSounds.clip = gunFire;
        gunSounds.Play();

        shootSound.clip = shootSounds[Random.Range(0, shootSounds.Length)];
        shootSound.pitch = 1 * Random.Range(.85f, 1.15f);
        shootSound.Play();

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
        gunSounds.clip = reloadBegin;
        gunSounds.Play();

        isReloading = true;
        anim.SetTrigger("ReloadBegin");
        Invoke("ReloadMidway", .9f);
    }
    private void ReloadMidway () {
        gunSounds.clip = reloadMidway;
        gunSounds.Play();

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
        if(hasReloadEnded) return;

        hasReloadEnded = true;
        gunSounds.clip = reloadEnd;
        gunSounds.Play();

        CancelInvoke("AddBullet");
        CancelInvoke("ReloadMidway");

        anim.ResetTrigger("ReloadBegin");
        anim.ResetTrigger("ReloadMidway");

        Invoke("StopReload", .833f);

        anim.SetTrigger("ReloadEnd");
    }

    private void StopReload () {
        hasReloadEnded = false;
        isReloading = false;
    }
}
