using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderPositionSetter : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetPosition;

    [SerializeField] private Animator fadeAnim;
    private bool wasActive;
    public void SetTargetPosition()
    {
        StartCoroutine("Move");
    }

    IEnumerator Move()
    {
        fadeAnim.Play("FadeUI");
        wasActive = target.gameObject.activeSelf;
        target.transform.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        target.transform.position = targetPosition;
        yield return null;
        target.transform.gameObject.SetActive(wasActive);
    }
}
