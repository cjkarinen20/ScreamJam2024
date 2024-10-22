using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetPosition;
    public void SetTargetPosition(){
        target.transform.position = targetPosition;
    }
}
