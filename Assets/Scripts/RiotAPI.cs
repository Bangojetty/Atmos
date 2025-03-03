using System.Collections;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

public class RiotAPI : MonoBehaviour {
    private string latestVersion;
    private string championDataURL;
    public MenuManager menuManager;
    
    public void GetChampions() {
        StartCoroutine(GetLatestPatchVersion());
    }
    
    
    private IEnumerator GetLatestPatchVersion() {
        string verstionURL = "https://ddragon.leagueoflegends.com/api/versions.json";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(verstionURL)) {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success) {
                JArray versions = JArray.Parse(webRequest.downloadHandler.text);
                latestVersion = versions[0].ToString();
                Debug.Log("Latest Patch: " + latestVersion);
                
                // Fetch Champ Data
                StartCoroutine(GetChampionData());

            } else {
                Debug.Log("Error fetching patch version: " + webRequest.error);
            }
            
        }
    }

    IEnumerator GetChampionData() {
        championDataURL = $"https://ddragon.leagueoflegends.com/cdn/{latestVersion}/data/en_US/champion.json";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(championDataURL)) {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success) {
                JObject championData = JObject.Parse(webRequest.downloadHandler.text);
                JObject champions = (JObject)championData["data"];

                List<Champion> championsList = new();

                
                menuManager.InitializeChampions(champions);
                
            } else {
                Debug.Log("Error fetching champion data: " + webRequest.error);
            }
        }
    }

    public void DownloadIcon(string champId, Champion champion) {
        StartCoroutine(DownloadIconEnum($"https://ddragon.leagueoflegends.com/cdn/{latestVersion}/img/champion/{champId}.png", champion));
    }

    private IEnumerator DownloadIconEnum(string url, Champion champ) {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url)) {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success) {
                Texture2D texture = DownloadHandlerTexture.GetContent(webRequest);
                champ.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                champ.SetIcon();
            } else {
                Debug.Log("Error downloading icon: " + webRequest.error);
            }
        }
    }
}
