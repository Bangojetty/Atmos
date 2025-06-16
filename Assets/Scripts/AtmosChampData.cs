using System;
using System.Collections.Generic;

[Serializable]
public class AtmosChampData {
    public string id;

    public List<MatchupData> matchups;


    public AtmosChampData(ChampionData champData) {
        matchups = new List<MatchupData>();
        id = champData.GetId();
    }
}
