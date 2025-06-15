using UnityEngine;
using UnityEngine.UI;

public class ChampionData {
    public string id { get; set; }
    public Sprite icon { get; set; }

    public ChampionData(string id) {
        this.id = id;
    }
    public string GetId() {
        return id == "MonkeyKing" ? "Wukong" : id;
    }
}
