using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneResetter : MonoBehaviour
{
    GameSceneManager gameSceneManager;

    // Start is called before the first frame update
    void Start()
    {
        gameSceneManager = GameSceneManager.Instance;
    }

    public void LoadMainMenu()
    {
        gameSceneManager.ReturnToMainMenu();
    }
}
