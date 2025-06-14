using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public RiotAPI riotApi;
    public GameObject mainMenu;
    public GameObject championMenu;
    public GameObject championContainer;
    public GameObject championPfb;
    public GameObject loadingScreen;
    private AtmosData atmosData;

    // ChampionsMenu
    public List<GameObject> champObjs = new();
    public TMP_InputField searchField;
    public Coroutine currentSearchCoroutine;

    // MainMenu
    public GameObject menuItems;

    public void Start() {
        riotApi = new RiotAPI();
        StartCoroutine(LoadData());
    }

    public void GetChampions() {
        ToggleChampMenu();
    }

    private void ToggleChampMenu() {
        mainMenu.SetActive(!mainMenu.activeSelf);
        championMenu.SetActive(!championMenu.activeSelf);
        LoadChampIcons();
    }

    private void LoadChampIcons() {
        foreach (var champ in atmosData.champions) {
            GameObject champObj = Instantiate(championPfb, championContainer.transform);
            champObj.name = champ.champName;
            if (champ.icon != null) {
                champObj.GetComponent<Image>().sprite = champ.icon;
            }
            champObjs.Add(champObj);
        }
    }

    private void DisplayMainMenu() {
        loadingScreen.SetActive(false);
        menuItems.SetActive(true);
    }


    private IEnumerator LoadData() {
        yield return StartCoroutine(riotApi.GetData(result => { atmosData = result; }));

        while (atmosData == null || !atmosData.isInitialized) {
            yield return null;
        }

        foreach (ChampionData champ in atmosData.champions) {
            yield return StartCoroutine(riotApi.DownloadIcon(champ.id, champ));
        }

        DisplayMainMenu();
    }

    public void FilterChampsByName() {
        if (currentSearchCoroutine != null) StopCoroutine(currentSearchCoroutine);
        currentSearchCoroutine = StartCoroutine(FilterAfterTypingPause());
    }

    public IEnumerator FilterAfterTypingPause() {
        string lastText = searchField.text;
        float timer = 0f;
        while (timer < 0.3f) { // adjust delay as needed
            if (searchField.text != lastText) {
                lastText = searchField.text;
                timer = 0f;
            }

            timer += Time.deltaTime;
            yield return null;
        }
        
        string search = lastText.ToLower();
        foreach (GameObject champObj in champObjs) {
            string champName = champObj.name.ToLower();
            champObj.SetActive(champName.Contains(search));
        }
    }


}
