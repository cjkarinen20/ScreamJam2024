using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetPosition;
    private bool wasActive;
    public void SetTargetPosition(){
        StartCoroutine("Move");
    }

    IEnumerator Move(){
        wasActive = target.gameObject.activeSelf;
        target.transform.gameObject.SetActive(false);
        yield return null;
        target.transform.position = targetPosition;
        yield return null;
        target.transform.gameObject.SetActive(wasActive);
    }
}
