using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class AtmosData {
    public List<ChampionData> champions = new();
    public bool isInitialized;

    public void InitializeChampions(JObject dataChamps, RiotAPI riotAPI) {
        foreach (var champ in dataChamps) {
            ChampionData newChampionData = new(champ.Value["id"].ToString());
            champions.Add(newChampionData);
        }
        isInitialized = true;
    }
}
