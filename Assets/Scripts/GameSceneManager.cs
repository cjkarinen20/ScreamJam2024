using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustenKinney.Essentials;
using UnityEngine.SceneManagement;

public class GameSceneManager : Singleton<GameSceneManager>
{
    public void StartGame()
    {
        SceneManager.LoadScene("GameLevel");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
