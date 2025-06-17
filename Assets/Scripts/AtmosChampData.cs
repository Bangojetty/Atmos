using System;
using System.Collections.Generic;

[Serializable]
public class AtmosChampData {
    public string id { get; set; }
    public Dictionary<string, MatchupData> idToMatchupData { get; set; }


    public AtmosChampData() {}

    public AtmosChampData(ChampionData champData) {
        idToMatchupData = new Dictionary<string, MatchupData>();
        id = champData.id;
    }
}
