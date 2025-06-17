using System;
using System.Collections.Generic;
using enums;

[Serializable]
public class MatchupData {
    public Difficulty difficulty { get; set; }
    public string playStyle { get; set; }
    public List<List<string>> blocks { get; set; }
    
    // non-json
    public int defaultBlockCount = 3;
    
    
    public MatchupData() {}
    
    
    public MatchupData(bool isDefault) {
        blocks = new List<List<string>>();
        for (int i = 0; i < defaultBlockCount; i++) {
            blocks.Add(new List<string>());
        }
        playStyle = "default";
        difficulty = Difficulty.EASY;
    }
}
