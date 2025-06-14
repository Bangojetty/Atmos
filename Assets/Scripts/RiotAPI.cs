using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

public class RiotAPI {
    private string latestVersion;
    private string championDataURL;
    private int requestTimer;


    public IEnumerator GetData(Action<AtmosData> callback) {
        using (UnityWebRequest webRequest =
               UnityWebRequest.Get("https://ddragon.leagueoflegends.com/api/versions.json")) {
            yield return webRequest.SendWebRequest();
            if (webRequest.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Error fetching patch version: " + webRequest.error);
                callback(null);
                yield break;
            }

            JArray versions = JArray.Parse(webRequest.downloadHandler.text);
            latestVersion = versions[0].ToString();
            Debug.Log("Latest Patch: " + latestVersion);
            championDataURL = $"https://ddragon.leagueoflegends.com/cdn/{latestVersion}/data/en_US/champion.json";
        }

        using (UnityWebRequest dataRequest = UnityWebRequest.Get(championDataURL)) {
            yield return dataRequest.SendWebRequest();

            if (dataRequest.result != UnityWebRequest.Result.Success) {
                Debug.LogError("Error fetching champion data: " + dataRequest.error);
                callback(null);
                yield break;
            }

            JObject championData = JObject.Parse(dataRequest.downloadHandler.text);
            JObject champions = (JObject)championData["data"];

            AtmosData atmosData = new();
            atmosData.InitializeChampions(champions, this);
            callback(atmosData);

        }
    }

    public IEnumerator DownloadIcon(string champId, ChampionData championData) {
        string url = $"https://ddragon.leagueoflegends.com/cdn/{latestVersion}/img/champion/{champId}.png";
        UnityWebRequest iconRequest = UnityWebRequestTexture.GetTexture(url);
        iconRequest.SetRequestHeader("User-Agent", "Mozilla/5.0");
        iconRequest.SetRequestHeader("Accept", "image/png");
        yield return iconRequest.SendWebRequest();

        if (iconRequest.result != UnityWebRequest.Result.Success) {
            Debug.LogError("Error downloading icon: " + iconRequest.error + " & " + iconRequest.responseCode);
        } else {
            Texture2D texture = DownloadHandlerTexture.GetContent(iconRequest);
            championData.icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}
