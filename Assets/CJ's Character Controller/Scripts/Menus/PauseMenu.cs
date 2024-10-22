using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public NewFPSController playerController;

    public bool isPaused;

    private AudioSource[] audioSources;
    private VideoPlayer[] videoSources;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
           if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
     public void PauseGame()
    {
        audioSources = FindObjectsOfType<AudioSource>();
        videoSources = FindObjectsOfType<VideoPlayer>();
        for(int i=0; i<audioSources.Length; i++){
            if(audioSources[i] != null){
                audioSources[i].Pause();
            }
        }
        for(int i=0; i<videoSources.Length; i++){
            if(videoSources[i] != null){
                videoSources[i].Pause();
            }
        }

        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        playerController.mouseLookEnabled = false;
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void ResumeGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        pauseMenu.SetActive(false);
        Cursor.visible = false;
        playerController.mouseLookEnabled = true;
        Time.timeScale = 1f;
        isPaused = false;

        if(audioSources != null){
            for(int i=0; i<audioSources.Length; i++){
            if(audioSources[i] != null){
                audioSources[i].UnPause();
            }
            }
            for(int i=0; i<audioSources.Length; i++){
                audioSources[i] = null;
            }
        }
        if(videoSources != null){
            for(int i=0; i<videoSources.Length; i++){
            if(videoSources[i] != null){
                videoSources[i].Play();
            }
            }
            for(int i=0; i<videoSources.Length; i++){
                videoSources[i] = null;
            }
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
