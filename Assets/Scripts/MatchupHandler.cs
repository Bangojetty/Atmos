using System;
using System.Collections.Generic;
using enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchupHandler : MonoBehaviour {
    public Image icon;
    public TMP_Text champNameText;
    public Image difficultyBorderImage;
    public GameObject startingItemsContainer;
    public Color orange = new Color(1f, 0.5f, 0f);

    public GameObject selectBorder;
    
    public MatchupData matchupData;

    public ChampPage champPage;


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

    public void AddBullet(int blockId, string bulletText) {
        matchupData.blocks[blockId].Add(bulletText);
    }

    public void RemoveBullet(int blockId, int bulletIndex) {
        matchupData.blocks[blockId].RemoveAt(bulletIndex);
    }

    public void Select() {
        selectBorder.SetActive(true);
        champPage.LoadMatchupData(this);
    }

    public void Deselect() {
        selectBorder.SetActive(false);
    }
    
}
