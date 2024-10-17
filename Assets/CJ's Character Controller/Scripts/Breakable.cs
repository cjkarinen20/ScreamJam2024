using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static iBreakable;

public class Breakable : MonoBehaviour, iBreakable
{
    [Header("Breakable Parameters")]
    [SerializeField] public BreakableType breakableType;
    [SerializeField] public GameObject breakable;

    private RequiredTool requiredTool;

    public void Start()
    {
        switch(breakableType)
        {
            case BreakableType.Boards:
                requiredTool = RequiredTool.Crowbar;
                break;
            case BreakableType.LadderPoint:
                requiredTool = RequiredTool.Ladder;
                break;
            case BreakableType.Chain:
                requiredTool = RequiredTool.Boltcutters;
                break;
            case BreakableType.Lock:
                requiredTool = RequiredTool.Key;
                break;
            default:
                break;
        }


    }
    public void Break()
    {
        breakable.gameObject.SetActive(false);
    }

    public void OnFocus()
    {
        Debug.Log("Breakable within interact range");
    }

    public void OnInteract()
    {
        Debug.Log("BREAK");
        Break();
    }

    public void OnLoseFocus()
    {
        Debug.Log("No longer in range");
    }

}
