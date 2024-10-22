using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleFullscreenController : MonoBehaviour
{
    [SerializeField] private AudioClip toggleSFX;

    private Toggle toggle;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(ToggleFullscreen);
        audioManager = AudioManager.Instance;
    }

    private void ToggleFullscreen(bool toggle)
    {
        Screen.fullScreen = toggle;

        audioManager.PlaySFX(toggleSFX);
    }
}
