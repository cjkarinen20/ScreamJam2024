using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MenuButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Color mouseOverColor;
    private Color defaultColor;

    private Button button;
    private TextMeshProUGUI textMesh;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        defaultColor = textMesh.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        textMesh.color = mouseOverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textMesh.color = defaultColor;
    }
}
