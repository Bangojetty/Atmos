using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class AtmosData {
    public List<ChampionData> champions = new();
    public bool isInitialized;

    public void InitializeChampions(JObject dataChamps, RiotAPI riotAPI) {
        foreach (var champ in dataChamps) {
            ChampionData newChampionData = new() {
                champName = champ.Value["name"].ToString(),
                id = champ.Value["id"].ToString()
            };
            champions.Add(newChampionData);
            Debug.Log($"{newChampionData.champName} ({newChampionData.id})");
        }
        isInitialized = true;
    }
}
