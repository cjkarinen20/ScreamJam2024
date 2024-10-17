using System;
using UnityEngine;
using UnityEngine.Events;
using static iBreakable;

public class Breakable : MonoBehaviour, iBreakable
{
    [Header("Breakable Parameters")]
    [SerializeField] private RequiredTool requiredTool;

    [SerializeField] private UnityEvent onBreak;

    private void Awake() {
        if(TryGetComponent<Rigidbody>(out Rigidbody rb)){// ! Use this to sleep rigidbodies and wake them up with the OnBreak
            rb.Sleep();
        }
    }

    public void Break()
    {
        onBreak?.Invoke();
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
