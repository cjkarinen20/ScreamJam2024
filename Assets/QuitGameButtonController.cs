using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGameButtonController : MonoBehaviour
{
    private GameSceneManager gameSceneManager;
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        gameSceneManager = GameSceneManager.Instance;
        button = GetComponent<Button>();
        button.onClick.AddListener(QuitGame);
    }

    // Update is called once per frame
    void QuitGame()
    {
        gameSceneManager.QuitGame();
    }
}
