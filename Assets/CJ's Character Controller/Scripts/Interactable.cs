using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public void OnInteract(bool calledFromPlayer = true);
    public void OnFocus();
    public void OnLoseFocus();
}
