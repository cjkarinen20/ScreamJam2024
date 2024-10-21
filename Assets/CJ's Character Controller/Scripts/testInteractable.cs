using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testInteractable : Interactable
{
    public void OnInteract(bool calledFromPlayer = true)
    {
        //Debug.Log("INTERACTED WITH " + gameObject.name);
    }
    public void OnFocus()
    {
        
        //Debug.Log("LOOKING AT " + gameObject.name);
    }
    public void OnLoseFocus()
    {
        //Debug.Log("STOPPED LOOKING AT " + gameObject.name);
    }

}
