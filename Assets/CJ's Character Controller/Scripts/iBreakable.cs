using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface iBreakable
{
    public enum RequiredTool
    {
        Crowbar,
        Ladder,
        Boltcutters,
        Key
    }
    public enum BreakableType
    {
        Boards,
        LadderPoint,
        Chain,
        Lock
    }
    public void OnInteract();
    public void Break();
    public void OnFocus();
    public void OnLoseFocus();
}
