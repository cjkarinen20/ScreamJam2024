using UnityEngine;
using UnityEngine.EventSystems;

public class Hoverable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    private Vector3 origScale = Vector3.zero, hoveredScale = Vector3.zero;
    public AudioSource hover, click;
    public float multiplier = 1.4f;
    [SerializeField] private bool putInFrontOnHover = true;
    private float origHoverPitch, origClickPitch;
    private bool isHovered = false;

    private void Awake() {
        AwakeValues();
    }

    public void AwakeValues(){
        if(hover != null) origHoverPitch = hover.pitch;
        if(click != null) origClickPitch = click.pitch;

        origScale = transform.localScale;

        isHovered = false;
        hoveredScale = origScale * multiplier;
    }

    private void OnDisable() {
        LeanTween.cancel(gameObject);
        transform.localScale = origScale;
        isHovered = false;
    }
    public void Hover(){
        if(isHovered) return;

        AwakeValues();
        
        isHovered = true;

        if(hover != null){
            hover.pitch = origHoverPitch * Random.Range(.8f, 1.2f);
            hover.Play();
        }
        if(putInFrontOnHover){
            transform.SetAsLastSibling();
        }
        LeanTween.cancel(gameObject);
        transform.LeanScale(hoveredScale, .3f).setEaseOutExpo().setIgnoreTimeScale(true);
    }

    public void ExitHover(bool forcedCall = false){
        if(!isHovered) return;
        isHovered = false;

        LeanTween.cancel(gameObject);
        transform.LeanScale(origScale, .3f).setEaseOutExpo().setIgnoreTimeScale(true);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Click();
    }

    public void Click(){
        if(click != null){
            click.pitch = origClickPitch * Random.Range(.8f, 1.2f);
            click.Play();
        }
        ExitHover();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Hover();
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        ExitHover();
    }

    public void OnSubmit (BaseEventData eventData)
    {
    	Click();
    }

    public void OnSelect(BaseEventData eventData = null)
    {
        Hover();
    }
    public void OnDeselect(BaseEventData eventData = null)
    {
        ExitHover(true);
    }
}
