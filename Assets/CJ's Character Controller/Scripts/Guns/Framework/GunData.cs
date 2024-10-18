using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "NewGunData", menuName = "Gun/GunData")]
public class GunData : ScriptableObject
{

    public LayerMask targetLayerMask;

    [Header("Fire Config")]
    public float shootingRange = 100;
    public float fireRate = 4;

    [Header("Reload Config")]
    public float magazineSize = 5;
    public float reloadTime = 2;

    [Header("Recoil")]
    public float recoilAmount = 5;

}
