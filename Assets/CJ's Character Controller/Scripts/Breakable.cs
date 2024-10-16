using System;
using UnityEngine;
using UnityEngine.Events;
using static iBreakable;

public class Breakable : MonoBehaviour, iBreakable
{
    [Header("Breakable Parameters")]
    [SerializeField] private RequiredTool requiredTool;

    [SerializeField] private UnityEvent onBreak;
    private Rigidbody rb;

    private void Awake() {
        if(TryGetComponent<Rigidbody>(out rb)){// ! Use this to sleep rigidbodies and wake them up with the OnBreak
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void Break()
    {
        if(rb != null) {
            rb.constraints = RigidbodyConstraints.None;
            rb.AddForce(new Vector3(UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f), UnityEngine.Random.Range(-2f, 2f)) * Time.fixedDeltaTime / 0.02f, ForceMode.Impulse);
        }

        onBreak?.Invoke();
        Destroy(this);
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
