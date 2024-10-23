using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject monster;
    [SerializeField] private NewFPSController newFPSController;
    [SerializeField] private Transform respawnPosition;

    private void OnEnable() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        monster.SetActive(false);
        newFPSController.gameObject.SetActive(false);
    }
    public void Respawn () {
        StartCoroutine("RespawnSetup");
    }

    public void Quit(){
        SceneManager.LoadScene("Main Menu");
    }

    IEnumerator RespawnSetup(){
        newFPSController.currentHealth = 100;
        newFPSController.currentStamina = 100;
        newFPSController.transform.position = respawnPosition.transform.position;
        monster.SetActive(false);
        yield return null;
        monster.SetActive(true);
        gameObject.SetActive(false);
        newFPSController.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
