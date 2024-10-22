using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAndRespawnTrigger : MonoBehaviour
{
    [SerializeField] private Animator fadeAnim;
    [SerializeField] private Transform respawnPosition;
    private Transform player;
    private NewFPSController newFPSController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            CancelInvoke();
            
            player = other.gameObject.transform;

            newFPSController = other.gameObject.GetComponent<NewFPSController>();

            NewFPSController.OnTakeDamage(0);

            fadeAnim.Rebind();
            fadeAnim.Play("FadeUI");

            Invoke("RepositionPlayer", .25f);

            newFPSController.canMove = false;
        }
    }

    private void RepositionPlayer () {
        newFPSController.enabled = false;
        newFPSController.canMove = true;
        player.transform.position = respawnPosition.transform.position + new Vector3(0, .9f, 0);
        player.transform.position = respawnPosition.transform.position + new Vector3(0, .9f, 0);
        player.transform.position = respawnPosition.transform.position + new Vector3(0, .9f, 0);
        player.transform.position = respawnPosition.transform.position + new Vector3(0, .9f, 0);
        newFPSController.enabled = true;
    }
}
