using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    public RiotAPI riotApi;

    public GameObject mainMenu;
    public GameObject championMenu;
    public GameObject championContainer;
    public GameObject championPfb;
    public void GetChampions() {
        ToggleChampMenu();
        riotApi.GetChampions();
    }

    private void ToggleChampMenu() {
        mainMenu.SetActive(!mainMenu.activeSelf);
        championMenu.SetActive(!championMenu.activeSelf);
    }

    public void InitializeChampions(JObject champions) {
        foreach (var champ in champions) {
            GameObject newChampObj = Instantiate(championPfb, championContainer.transform);
            Champion newChampion = newChampObj.GetComponent<Champion>();
            newChampion.name = champ.Value["name"].ToString();
            newChampion.id = champ.Value["id"].ToString();
            riotApi.DownloadIcon(newChampion.id, newChampion);
            
            Debug.Log($"{newChampion.name} ({newChampion.id})");
        }
    }
}
