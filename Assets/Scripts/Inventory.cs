using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject[] guns;

    private void Awake() {
        for(int i = 0; i < guns.Length; i++) {
            SetGunState(i, false);
        }
    }

    private void Update() {
        if(Input.GetKeyDown("1")){
            SetGunState(0, true);
            SetGunState(1, false);
        }
        if(Input.GetKeyDown("2")){
            SetGunState(0, false);
            SetGunState(1, true);
        }
    }

    private void SetGunState (int index, bool state) {
        guns[index].SetActive(state);
    }
}
