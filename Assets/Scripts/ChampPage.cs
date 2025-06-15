using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChampPage : MonoBehaviour {
    public Image icon;
    public TMP_Text title;
    private AtmosChampData atmosChampData;



    public void LoadChampPage(ChampionData champData) {
        string atmosChampFolderPath = Application.persistentDataPath + "/atmos/champion/";
        if (!Directory.Exists(atmosChampFolderPath)) {
            Directory.CreateDirectory(atmosChampFolderPath);
        }
        string champPath = Path.Combine(atmosChampFolderPath, champData.GetId() + ".json");

        if (File.Exists(champPath)) {
            string json = File.ReadAllText(champPath);
            atmosChampData = JsonUtility.FromJson<AtmosChampData>(json);
            Debug.Log("Loaded saved champion data: "+ champData.GetId());
        } else {
            atmosChampData = new AtmosChampData(champData);
            string json = JsonUtility.ToJson(atmosChampData, true);
            File.WriteAllText(champPath, json);
            Debug.Log("Created and saved new AtmosChampData for: " + champData.GetId());
        }
        icon.sprite = champData.icon;
        title.text = champData.GetId();
    }
    
}
