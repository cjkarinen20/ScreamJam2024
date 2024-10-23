using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ItemPickup : MonoBehaviour, Interactable
{
    public enum ItemType{CROWBAR, LADDER, KEY, BOLTCUTTER}
    public ItemType itemType;
    [SerializeField] private AudioSource pickup;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private UnityEvent onPickup;
    
    public void OnInteract(bool calledFromPlayer = true){
        if(itemType == ItemType.CROWBAR) InventorySystem.instance.GiveCrowbar();
        if(itemType == ItemType.BOLTCUTTER) InventorySystem.instance.GiveBoltCutters();
        if(itemType == ItemType.KEY) InventorySystem.instance.GiveKey();
        if(itemType == ItemType.LADDER) InventorySystem.instance.GiveLadder();

        onPickup?.Invoke();

        Destroy(this.gameObject);

        pickup.clip = pickupSound;
        pickup.Play();
    }

    public void OnFocus(){

    }
    public void OnLoseFocus(){
        
    }
}
