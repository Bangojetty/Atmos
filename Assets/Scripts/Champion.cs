using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Champion : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {
    public Image icon;
    
    // border
    public Image borderImage;
    public Sprite greenBorder;
    public Sprite redBorder;
    
    
    public void OnPointerEnter(PointerEventData eventData) {
        borderImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        borderImage.sprite = greenBorder;
        borderImage.enabled = false;
        icon.color = Color.white;
    }

    public void OnPointerDown(PointerEventData eventData) {
        borderImage.sprite = redBorder;
        icon.color = new Color32(155, 155, 155, 255);
    }

    public void OnPointerUp(PointerEventData eventData) {
        borderImage.sprite = greenBorder;
        icon.color = Color.white;
    }
}
