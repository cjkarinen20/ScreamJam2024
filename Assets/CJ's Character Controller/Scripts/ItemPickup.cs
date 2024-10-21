using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, Interactable
{
    public enum ItemType{CROWBAR, LADDER, KEY, BOLTCUTTER}
    public ItemType itemType;
    [SerializeField] private AudioSource pickup;
    [SerializeField] private AudioClip pickupSound;
    
    public void OnInteract(bool calledFromPlayer = true){
        if(itemType == ItemType.CROWBAR) InventorySystem.instance.GiveCrowbar();
        if(itemType == ItemType.BOLTCUTTER) InventorySystem.instance.GiveBoltCutters();
        if(itemType == ItemType.KEY) InventorySystem.instance.GiveKey();

        Destroy(this.gameObject);

        pickup.clip = pickupSound;
        pickup.Play();
    }

    public void OnFocus(){

    }
    public void OnLoseFocus(){
        
    }
}
