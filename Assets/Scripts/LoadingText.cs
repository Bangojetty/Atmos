using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class LoadingText : MonoBehaviour {
    public string fullString;
    [FormerlySerializedAs("tmpText")] public TMP_Text text;
    void Start() {
        text = GetComponent<TMP_Text>();
        StartCoroutine(IterateText());
    }

    private IEnumerator IterateText() {
        while (true) {
            text.text = "";
            string tempString = fullString;
            // this scales the text speed based on the string length and clamps it so it doesn't go too fast
            float delay = 0.1f * Mathf.Pow(10f / tempString.Length, 0.6f);
            delay = Mathf.Clamp(delay, 0.06f, 0.1f);
            foreach (char c in tempString) {
                yield return new WaitForSeconds(delay);
                text.text += c;
            }
            yield return new WaitForSeconds(0.5f);
        }
        
    }

}
