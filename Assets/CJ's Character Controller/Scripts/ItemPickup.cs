using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour, Interactable
{
    public enum ItemType{CROWBAR, LADDER, KEY, BOLTCUTTER}
    public ItemType itemType;
    public void OnInteract(){
        if(itemType == ItemType.CROWBAR) InventorySystem.instance.GiveCrowbar();
        if(itemType == ItemType.BOLTCUTTER) InventorySystem.instance.GiveBoltCutters();

        Destroy(this.gameObject);
    }

    public void OnFocus(){

    }
    public void OnLoseFocus(){
        
    }
}
