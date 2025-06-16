using System;
using System.Collections.Generic;

[Serializable]
public class AtmosChampData {
    public string id;
    public Dictionary<string, MatchupData> idToMatchupData;


    public AtmosChampData(ChampionData champData) {
        idToMatchupData = new Dictionary<string, MatchupData>();
        id = champData.id;
    }
}
