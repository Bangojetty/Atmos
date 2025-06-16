using System.Collections.Generic;
using System.IO;
using enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChampPage : MonoBehaviour {
    public Image icon;
    public TMP_Text title;
    private AtmosChampData atmosChampData;
    
    public GameObject matchupContainerObj;
    public GameObject matchupPfb;
    
    public List<GameObject> matchupObjs = new();
    private InputField searchField;
    
    private List<string> loadedMatchups = new();
    



    public void LoadChampPage(ChampionData champData, AtmosData atmosData) {
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
        icon.preserveAspect = true;
        title.text = champData.GetId();
        
        // only load matchups if they aren't already loaded
        if (loadedMatchups.Contains(champData.id)) return;
        LoadMatchups(atmosData);
        loadedMatchups.Add(champData.id);
    }


    public void LoadMatchups(AtmosData atmosData) {
        // TODO check for first time load ->
        // you shouldn't initialize matchups for champs that the use has not yet selected
        foreach (ChampionData champData in atmosData.champions) {
            GameObject newMatchupObj = Instantiate(matchupPfb, matchupContainerObj.transform);
            Matchup newMatchup = newMatchupObj.GetComponent<Matchup>();
            newMatchup.icon.sprite = champData.icon;
            newMatchup.name = champData.GetId();
            newMatchup.champNameText.text = champData.GetId();
            newMatchup.SetDifficulty(Difficulty.EASY);
            matchupObjs.Add(newMatchupObj);
        }
    }

    public void FilterMatchupsByName() {
        foreach (GameObject matchupObj in matchupObjs) {
            matchupObj.SetActive(matchupObj.name.Contains(searchField.text));
        }
    }
    
}
