using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Champion : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    // border
    public Image borderImage;
    public Sprite greenBorder;
    public Sprite orangeBorder;
    
    // data
    public ChampionData champData;
    public void OnPointerEnter(PointerEventData eventData) {
        borderImage.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        borderImage.sprite = greenBorder;
        borderImage.enabled = false;
    }

    public void OnClick() {
        borderImage.sprite = greenBorder;
        borderImage.enabled = false;
        GameObject.Find("MenuManager").GetComponent<MenuManager>().OpenChampPage(champData);
    }

    public void OnPointerDown(PointerEventData eventData) {
        borderImage.sprite = orangeBorder;
    }
}
