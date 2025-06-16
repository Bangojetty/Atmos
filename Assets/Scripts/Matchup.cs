using System;
using System.Collections.Generic;
using enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Matchup : MonoBehaviour {
    public Image icon;
    public TMP_Text champNameText;
    public Image difficultyBorderImage;
    public GameObject startingItemsContainer;
    public Color orange = new Color(1f, 0.5f, 0f);
    
    public MatchupData matchupData;
    

    public void SetDifficulty(Difficulty newDifficulty) {
        matchupData.difficulty = newDifficulty;
        switch (newDifficulty) {
            case Difficulty.EASY:
                difficultyBorderImage.color = Color.green;
                break;
            case Difficulty.NORMAL:
                difficultyBorderImage.color = Color.yellow;
                break;
            case Difficulty.HARD: 
                difficultyBorderImage.color = orange;
                break;
            case Difficulty.EXPERT:
                difficultyBorderImage.color = Color.red;
                break;
            default:
                Debug.LogError("Unknown difficulty");
                break;
        }
    }
    
    
}
