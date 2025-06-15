using System;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class RiotAPI {
    private string cdnURL;
    private int requestTimer;
    private bool dataIsLoaded;
    
    private readonly string versionPath = Application.persistentDataPath + "/riot_version.txt";
    private readonly string championCachePath = Application.persistentDataPath + "/champions.txt";
    private readonly string iconFolderPath = Application.persistentDataPath + "/icons/";

    public IEnumerator GetData(Action<AtmosData> callback, MenuManager menuManager) {
        dataIsLoaded = true;
        if (!Directory.Exists(iconFolderPath)) {
            Directory.CreateDirectory(iconFolderPath);
        }
        string latestVersion = null;
        using (UnityWebRequest versionRequest =
               UnityWebRequest.Get("https://ddragon.leagueoflegends.com/api/versions.json")) {
            yield return versionRequest.SendWebRequest();
            if (versionRequest.result == UnityWebRequest.Result.Success) {
                JArray versions = JArray.Parse(versionRequest.downloadHandler.text);
                latestVersion = versions[0].ToString();
                Debug.Log("Latest Patch: " + latestVersion);
                cdnURL = $"https://ddragon.leagueoflegends.com/cdn/{latestVersion}";
            } else {
                Debug.LogError("Error fetching patch version: " + versionRequest.error);
                callback(null);
                yield break;
            }
        }
        
        string localVersion = File.Exists(versionPath) ? File.ReadAllText(versionPath) : null;

        if (localVersion == latestVersion && File.Exists(championCachePath)) {
            Debug.Log("Loading champion data from local cache.");
            menuManager.loadingText.fullString = "Loading Champion Data...";
            string json = File.ReadAllText(championCachePath);
            JObject championData = JObject.Parse(json);
            JObject champions = (JObject)championData["data"];
            
            AtmosData cachedData = new AtmosData();
            cachedData.InitializeChampions(champions, this);
            callback(cachedData);
            yield break;
        }
        
        // if you don't have the latest version data, fetch fresh data and cache it
        using (UnityWebRequest dataRequest = UnityWebRequest.Get(cdnURL + "/data/en_US/champion.json")) {
            yield return dataRequest.SendWebRequest();

            if (dataRequest.result == UnityWebRequest.Result.Success) {
                Debug.Log("Downloading fresh champion data.");
                menuManager.loadingText.fullString = "Updating To Latest Patch...";
                string jsonText = dataRequest.downloadHandler.text;
                
                File.WriteAllText(versionPath, latestVersion);
                File.WriteAllText(championCachePath, jsonText);
                
                JObject championData = JObject.Parse(jsonText);
                JObject champions = (JObject)championData["data"];

                AtmosData atmosData = new();
                atmosData.InitializeChampions(champions, this);
                callback(atmosData);
                yield break;
            }
            Debug.LogError("Error fetching champion data: " + dataRequest.error);
            callback(null);
        }
    }

    public IEnumerator LoadOrDownloadIcon(string champId, ChampionData championData, MenuManager menuManager) {
        if (!dataIsLoaded) Debug.LogError("Data was not loaded before attempting to load icons");
        
        string iconPath = Path.Combine(iconFolderPath, $"{champId}.png");

        if (File.Exists(iconPath)) {
            // load from file
            menuManager.loadingText.fullString = "Loading Icons...";
            byte[] fileData = File.ReadAllBytes(iconPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            championData.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            yield break;
        }
        
        // otherwise, download and save icon
        using UnityWebRequest iconRequest =
            UnityWebRequestTexture.GetTexture(cdnURL + $"/img/champion/{champId}.png");
        iconRequest.SetRequestHeader("User-Agent", "Mozilla/5.0");
        iconRequest.SetRequestHeader("Accept", "image/png");
            
        yield return iconRequest.SendWebRequest();

        if (iconRequest.result == UnityWebRequest.Result.Success) {
            Texture2D texture = DownloadHandlerTexture.GetContent(iconRequest);
            championData.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            
            // save png to disk
            byte[] pngData = texture.EncodeToPNG();
            File.WriteAllBytes(iconPath, pngData);
        } else {
            Debug.LogError("Error downloading icon: " + iconRequest.error + " & " + iconRequest.responseCode);
        }
    }
}
