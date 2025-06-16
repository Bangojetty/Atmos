using System;
using System.Collections.Generic;
using enums;

[Serializable]
public class MatchupData {
    public string id;
    public Difficulty difficulty;
    public string playStyle;
    public List<string> notes;
    public List<string> toDos;
    public List<string> notToDos;

    public MatchupData() {
        notes = new List<string>();
        toDos = new List<string>();
        notToDos = new List<string>();
        playStyle = "default";
        difficulty = Difficulty.EASY;
    }
}
