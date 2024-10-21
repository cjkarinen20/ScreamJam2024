using System;
using UnityEngine;
using UnityEngine.Events;
using static iBreakable;

public class Breakable : MonoBehaviour, iBreakable
{
    public RequiredTool requiredTool;

    [SerializeField] private UnityEvent onBreak;
    private Rigidbody rb;
    public bool hasBeenBroken {private set; get;}

    private void Awake() {
        hasBeenBroken = false;
        if(TryGetComponent<Rigidbody>(out rb)){
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void Break()
    {
        if(hasBeenBroken) return;

        hasBeenBroken = true;
        
        float delay = 0;
        if(requiredTool == RequiredTool.Crowbar) delay = .7f;
        if(requiredTool == RequiredTool.Boltcutters) delay = 1.15f;

        Invoke("FullBreak", delay);
    }

    private void FullBreak () {
        if(TryGetComponent<BoxCollider>(out BoxCollider collider)){// ! ONLY USE BOX COLLIDERS FOR BREAKABLES
            collider.layerOverridePriority = 10000;
            collider.excludeLayers |= (1 << LayerMask.NameToLayer("Player"));
            collider.size /= 2;
        }
        
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
