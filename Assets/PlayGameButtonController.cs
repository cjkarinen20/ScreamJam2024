using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayGameButtonController : MonoBehaviour
{
    private GameSceneManager gameSceneManager;
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        gameSceneManager = GameSceneManager.Instance;
        button = GetComponent<Button>();
        button.onClick.AddListener(PlayGame);
    }

    // Update is called once per frame
    void PlayGame()
    {
        gameSceneManager.StartGame();
    }
}
