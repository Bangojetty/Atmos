using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingText : MonoBehaviour {
    public string fullString;
    private TMP_Text text;
    void Start() {
        text = GetComponent<TMP_Text>();
        StartCoroutine(IterateText());
    }

    private IEnumerator IterateText() {
        while (true) {
            text.text = "";
            foreach (char c in fullString) {
                yield return new WaitForSeconds(0.1f);
                text.text += c;
            }
            yield return new WaitForSeconds(0.5f);
        }
        
    }

}
