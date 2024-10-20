using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;
    [SerializeField] private GameObject[] guns;
    [SerializeField] private GameObject gunArmsGroup, crowbarArms, boltCutterArms;
    private Animator crowbarAnim, boltCutterAnim;
    public bool hasCrowbar {private set; get;}
    public bool hasBoltCutters {private set; get;}

    private void Awake() {
        if(instance == null){
            instance = this;
        }
        hasCrowbar = false;
        hasBoltCutters = false;
        
        for(int i = 0; i < guns.Length; i++) {
            SetGunState(i, false);
        }
        gunArmsGroup.SetActive(true); 

        boltCutterArms.SetActive(false);
        crowbarArms.SetActive(false);
    }

    private void Start() {
        crowbarAnim = crowbarArms.GetComponent<Animator>();
        boltCutterAnim = boltCutterArms.GetComponent<Animator>();
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

    public void GiveCrowbar(){
        hasCrowbar = true;
    }

    public void GiveBoltCutters(){
        hasBoltCutters = true;
    }

    public void UseCrowbar () {
        CancelInvoke();

        crowbarAnim.Rebind();
        crowbarAnim.Play("CrowbarUse");

        gunArmsGroup.SetActive(false);

        boltCutterArms.SetActive(false);
        crowbarArms.SetActive(true);

        Invoke("ReenableArms", 1.583f);
    }
    public void UseBoltCutter () {
        CancelInvoke();

        boltCutterAnim.Rebind();
        boltCutterAnim.Play("BoltCutterUse");

        gunArmsGroup.SetActive(false);

        boltCutterArms.SetActive(true);
        crowbarArms.SetActive(false);

        Invoke("ReenableArms", 2.317f);
    }

    private void ReenableArms () {
        gunArmsGroup.SetActive(true); 

        boltCutterArms.SetActive(false);
        crowbarArms.SetActive(false);
    }
}