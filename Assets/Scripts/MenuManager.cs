using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public RiotAPI riotApi;
    public GameObject mainMenu;
    public GameObject championMenu;
    public GameObject championContainer;
    public GameObject championPfb;
    public GameObject loadingScreen;
    public LoadingText loadingText;
    private AtmosData atmosData;
    
    // MainMenu
    public GameObject menuItems;

    // ChampionsMenu
    public List<GameObject> champObjs = new();
    public TMP_InputField searchField;
    public Coroutine currentSearchCoroutine;
    
    // ChampPage
    public GameObject champPageObj;
    public ChampPage champPage;
    public bool iconsAreLoaded;
    
    public void Start() {
        riotApi = new RiotAPI();
        StartCoroutine(LoadData());
    }

    public void NavMain() {
        mainMenu.SetActive(true);
        championMenu.SetActive(false);
    }
    
    public void NavChampMenu() {
        if(!iconsAreLoaded) LoadChampIcons();
        mainMenu.SetActive(false);
        champPageObj.SetActive(false);
        championMenu.SetActive(true);
    }

    private void LoadChampIcons() {
        foreach (var champ in atmosData.champions) {
            GameObject champObj = Instantiate(championPfb, championContainer.transform);
            champObj.GetComponent<Champion>().champData = champ;
            champObj.name = champ.GetId();
            if (champ.icon != null) {
                champObj.GetComponent<Button>().image.sprite = champ.icon;
            }
            champObjs.Add(champObj);
        }
        iconsAreLoaded = true;
    }

    private void DisplayMainMenu() {
        loadingScreen.SetActive(false);
        menuItems.SetActive(true);
    }


    private IEnumerator LoadData() {
        yield return StartCoroutine(riotApi.GetData(result => { atmosData = result; }, this));

        while (atmosData == null || !atmosData.isInitialized) {
            yield return null;
        }

        foreach (ChampionData champ in atmosData.champions) {
            yield return StartCoroutine(riotApi.LoadOrDownloadIcon(champ.id, champ, this));
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

    public void OpenChampPage(ChampionData champData) {
        champPageObj.SetActive(true);
        championMenu.SetActive(false);
        champPage.LoadChampPage(champData, atmosData);
    }
    
}
