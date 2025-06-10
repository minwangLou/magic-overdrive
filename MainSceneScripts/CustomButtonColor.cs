using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButtonColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image targetImage;

    public Color normalColor;
    public Color hoverColor;
    public Color pressedColor;


    private bool isPointerOver = false;

    void Start()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        targetImage.color = normalColor;
        /*
        if (normalColor == null && hoverColor == null && pressedColor == null)
        {
            normalColor = new Color32(0x20, 0x20, 0x20, 0xFF);     // #202020
            hoverColor = new Color32(0x2B, 0x2B, 0x2B, 0xFF);      // #2B2B2B
            pressedColor = new Color32(0x78, 0x78, 0x78, 0xFF);    // #787878
         }
        */
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        targetImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        targetImage.color = normalColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetImage.color = pressedColor;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetImage.color = isPointerOver ? hoverColor : normalColor;
    }
}
