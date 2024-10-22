using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color mouseOverColor;
    [SerializeField] private AudioClip mouseOverSFX;
    [SerializeField] private AudioClip onClickSFX;
    private Color defaultColor;

    private Button button;
    private TextMeshProUGUI textMesh;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        defaultColor = textMesh.color;
        audioManager = AudioManager.Instance;

        button.onClick.AddListener(PlayOnClickSFX);
    }

    private void PlayOnClickSFX()
    {
        if(onClickSFX != null){
            audioManager.PlaySFX(onClickSFX);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        textMesh.color = mouseOverColor;
        
        if(mouseOverSFX != null){
            audioManager.PlaySFXRandomPitch(mouseOverSFX);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textMesh.color = defaultColor;
    }
}
