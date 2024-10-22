using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleWallTriggerVolume : MonoBehaviour
{
    [SerializeField] private bool targetActiveState;
    [SerializeField] private GameObject blocker;

    private void OnTriggerEnter(Collider other)
    {
        blocker.SetActive(targetActiveState);
    }
}
