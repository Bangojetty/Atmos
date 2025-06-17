using System.Collections.Generic;
using System.IO;
using System.Linq;
using enums;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ChampPage : MonoBehaviour {
    public Image icon;
    public TMP_Text title;
    public AtmosChampData atmosChampData;
    private InputField searchField;
    
    // matchups
    public GameObject matchupContainerObj;
    public GameObject matchupPfb;
    private List<GameObject> matchupObjs = new();
    private Dictionary<string, MatchupHandler> matchupHandlerById = new();
    private MatchupHandler currentMatchupHandler;
    
    // blocks
    public GameObject infoBlockContainer;
    public GameObject infoBlockPfb;
    private bool defaultBlocksAreInitialized;
    
    // bullets    
    public GameObject bulletInputPfb;
    public GameObject bulletPfb;
    public List<BulletContainer> bulletContainers = new();


    public void LoadChampPage(ChampionData champData, AtmosData atmosData) {
        string atmosChampFolderPath = Application.persistentDataPath + "/atmos/champion/";
        if (!Directory.Exists(atmosChampFolderPath)) {
            Directory.CreateDirectory(atmosChampFolderPath);
        }
        string champPath = Path.Combine(atmosChampFolderPath, champData.id + ".json");

        if (File.Exists(champPath)) {
            string json = File.ReadAllText(champPath);
            atmosChampData = JsonConvert.DeserializeObject<AtmosChampData>(json);
            Debug.Log("Loaded saved champion data: "+ utils.NameFromId(champData.id));
        } else {
            atmosChampData = new AtmosChampData(champData);
            string json = JsonConvert.SerializeObject(atmosChampData);
            File.WriteAllText(champPath, json);
            Debug.Log("Created and saved new AtmosChampData for: " + utils.NameFromId(champData.id));
        }
        icon.sprite = champData.icon;
        icon.preserveAspect = true;
        title.text = utils.NameFromId(champData.id);
        LoadMatchups(atmosData);
    }

    private void InitializeDefaultBlocks() {
        CreateBlock("NotesBlock", "Notes: ");
        CreateBlock("ToDoBlock", "To Do: ");
        CreateBlock("NotToDoBlock", "Not To Do: ");
        defaultBlocksAreInitialized = true;
    }

    private void CreateBlock(string blockName, string blockTitle) {
        GameObject notesBlockObj = Instantiate(infoBlockPfb, infoBlockContainer.transform);
        notesBlockObj.name = blockName;
        InfoBlock notesInfoBlock = notesBlockObj.GetComponent<InfoBlock>();
        notesInfoBlock.titleText.text = blockTitle;
        bulletContainers.Add(notesInfoBlock.bulletContainer);
        notesInfoBlock.bulletContainer.blockId = bulletContainers.IndexOf(notesInfoBlock.bulletContainer);
    }


    public void LoadMatchups(AtmosData atmosData) {
        foreach (ChampionData champData in atmosData.champions) {
            MatchupData focusMatchupData;
            if (!atmosChampData.idToMatchupData.TryGetValue(champData.id, out var value)) {
                focusMatchupData = new MatchupData(true);
                atmosChampData.idToMatchupData.Add(champData.id, focusMatchupData);
            } else focusMatchupData = value;
            
            
            if (matchupHandlerById.TryGetValue(champData.id, out var focusMatchupHandler)) {
                focusMatchupHandler.matchupData = focusMatchupData;
                Debug.Assert(focusMatchupHandler.champPage != null, 
                    "no champ page found for matchuphandler for champ: " + utils.NameFromId(champData.id));
                return;
            } 
            GameObject newMatchupObj = Instantiate(matchupPfb, matchupContainerObj.transform);
            newMatchupObj.name = utils.NameFromId(champData.id);
            matchupObjs.Add(newMatchupObj);
            MatchupHandler newMatchupHandler = newMatchupObj.GetComponent<MatchupHandler>();
            newMatchupHandler.champPage = this;
            newMatchupHandler.matchupData = focusMatchupData;
            newMatchupHandler.icon.sprite = champData.icon;
            newMatchupHandler.champNameText.text = utils.NameFromId(champData.id);
            newMatchupHandler.SetDifficulty(focusMatchupData.difficulty);
        }
    }

    public void FilterMatchupsByName() {
        foreach (GameObject matchupObj in matchupObjs) {
            matchupObj.SetActive(matchupObj.name.Contains(searchField.text));
        }
    }


    private void SaveAtmosChampData() {
        string json = JsonConvert.SerializeObject(atmosChampData);
        string atmosChampFolderPath = Application.persistentDataPath + "/atmos/champion/";
        string champPath = Path.Combine(atmosChampFolderPath, atmosChampData.id + ".json");
        File.WriteAllText(champPath, json);
        Debug.Log("Created and saved new AtmosChampData for: " + utils.NameFromId(atmosChampData.id));
    }

    public void NavChampMenu() {
        SaveAtmosChampData();
        GameObject.Find("MenuManager").GetComponent<MenuManager>().NavChampMenu();
    }

    public void AttemptToAddBullet(GameObject bulletContainer, int blockId) {
        GameObject newBulletInput = Instantiate(bulletInputPfb, bulletContainer.transform);
        newBulletInput.GetComponent<BulletInput>().blockId = blockId;
    }

    public void CreateBullet(int blockId, string bulletText, bool isData) {
        GameObject newBullet = Instantiate(bulletPfb, bulletContainers[blockId].transform);
        newBullet.GetComponent<TMP_Text>().text = "-" + bulletText;
        if(!isData) currentMatchupHandler.AddBullet(blockId, bulletText);
    }

    public void LoadMatchupData(MatchupHandler matchupHandler) {
        if (!defaultBlocksAreInitialized) {
            InitializeDefaultBlocks();
        }
        if (currentMatchupHandler != null) {
            currentMatchupHandler.Deselect();
            SaveAtmosChampData();
        }
        currentMatchupHandler = matchupHandler;
        for (int i = 0; i < bulletContainers.Count; i++) {
            foreach (Transform child in bulletContainers[i].transform) {
                Destroy(child.gameObject);
            }
            foreach (string bullet in matchupHandler.matchupData.blocks[i]) {
                CreateBullet(i, bullet, true);
            }
        }
    }
}
