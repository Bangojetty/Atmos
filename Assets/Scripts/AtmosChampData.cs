using System;
using System.Collections.Generic;

[Serializable]
public class AtmosChampData {
    public string id;

    public List<MatchUp> matchups;


    public AtmosChampData(ChampionData champData) {
        matchups = new List<MatchUp>();
        id = champData.GetId();
    }
}
