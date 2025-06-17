using System;
using TMPro;
using UnityEngine;

public class BulletInput : MonoBehaviour {
    public float baseHeight;
    public float padding;

    public TMP_InputField inputField;
    public RectTransform inputRectTransform;
    public RectTransform blockRectTransform;
    public int blockId;

    private TMP_Text textComponent;

    public ChampPage champPage;
    
    private void Start() {
        champPage = GameObject.Find("ChampPage").GetComponent<ChampPage>(); 
        textComponent = inputField.textComponent;
    }


    void Update() {
        float preferredHeight = inputField.textComponent.preferredHeight;
        textComponent.ForceMeshUpdate();
        float targetHeight = Mathf.Max(baseHeight, preferredHeight + padding);
        Debug.Log("Preferred Height: " + preferredHeight);
        Debug.Log("Target Height: " + targetHeight);
        inputRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
        blockRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, targetHeight);
    }


    public void Submit() {
        champPage.CreateBullet(blockId, inputField.text, false);
        Destroy(gameObject);
    }
}
