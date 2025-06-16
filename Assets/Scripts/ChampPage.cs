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

    public Dictionary<string, Matchup> matchupById = new();
    private InputField searchField;
    



    public void LoadChampPage(ChampionData champData, AtmosData atmosData) {
        string atmosChampFolderPath = Application.persistentDataPath + "/atmos/champion/";
        if (!Directory.Exists(atmosChampFolderPath)) {
            Directory.CreateDirectory(atmosChampFolderPath);
        }
        string champPath = Path.Combine(atmosChampFolderPath, champData.id + ".json");

        if (File.Exists(champPath)) {
            string json = File.ReadAllText(champPath);
            atmosChampData = JsonUtility.FromJson<AtmosChampData>(json);
            Debug.Log("Loaded saved champion data: "+ utils.NameFromId(champData.id));
        } else {
            atmosChampData = new AtmosChampData(champData);
            string json = JsonUtility.ToJson(atmosChampData, true);
            File.WriteAllText(champPath, json);
            Debug.Log("Created and saved new AtmosChampData for: " + utils.NameFromId(champData.id));
        }
        icon.sprite = champData.icon;
        icon.preserveAspect = true;
        title.text = utils.NameFromId(champData.id);
        LoadMatchups(atmosData);
    }


    public void LoadMatchups(AtmosData atmosData) {
        foreach (ChampionData champData in atmosData.champions) {
            if (matchupById.ContainsKey(champData.id)) {
                // populate from atmoschampdata
                return;
            } 
            GameObject newMatchupObj = Instantiate(matchupPfb, matchupContainerObj.transform);
            Matchup newMatchup = newMatchupObj.GetComponent<Matchup>();
            MatchupData newMData = new MatchupData();
            newMatchup.matchupData = newMData;
            newMatchup.icon.sprite = champData.icon;
            newMatchupObj.name = utils.NameFromId(champData.id);
            newMatchup.champNameText.text = utils.NameFromId(champData.id);
            newMatchup.SetDifficulty(Difficulty.EASY);
            matchupObjs.Add(newMatchupObj);
            atmosChampData.idToMatchupData.Add(champData.id, newMatchup.matchupData);
        }
    }

    public void FilterMatchupsByName() {
        foreach (GameObject matchupObj in matchupObjs) {
            matchupObj.SetActive(matchupObj.name.Contains(searchField.text));
        }
    }


    private void SaveAtmosChampData() {
        string json = JsonUtility.ToJson(atmosChampData, true);
        string atmosChampFolderPath = Application.persistentDataPath + "/atmos/champion/";
        string champPath = Path.Combine(atmosChampFolderPath, atmosChampData.id + ".json");
        File.WriteAllText(champPath, json);
        Debug.Log("Created and saved new AtmosChampData for: " + utils.NameFromId(atmosChampData.id));
    }

    public void NavChampMenu() {
        SaveAtmosChampData();
        GameObject.Find("MenuManager").GetComponent<MenuManager>().NavChampMenu();
    }
    
}
