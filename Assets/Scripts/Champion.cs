using System;
using UnityEngine;
using UnityEngine.UI;

public class Champion : MonoBehaviour {
    public string name;
    public string id;
    public Sprite icon;

    public void SetIcon() {
        gameObject.GetComponent<Image>().sprite = icon;
    }
    
}
