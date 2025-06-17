using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButtonColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image targetImage;
    private CanvasGroup canvasbutton;

    public Color normalColor;
    public Color hoverColor;
    public Color pressedColor;

    private bool isPointerOver = false;

    void Start()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        canvasbutton = GetComponent<CanvasGroup>();

        targetImage.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsInteractable()) return;

        isPointerOver = true;
        targetImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsInteractable()) return;

        isPointerOver = false;
        targetImage.color = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsInteractable()) return;

        targetImage.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!IsInteractable()) return;

        targetImage.color = isPointerOver ? hoverColor : normalColor;
    }

    private bool IsInteractable()
    {
        if (canvasbutton == null)
            return true; 
        if (!canvasbutton.interactable)
        {
            targetImage.color = normalColor; 
            return false;
        }
        return true;
    }
}