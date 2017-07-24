using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Twitch.Net {
    public class TwitchApiRequester : MonoBehaviour {
        private readonly string clientId = "fsbs0ur09zmbi3drjh9f4b8snelc17";

        public void GetTrendingClips(string gameTitle, int limit, Action<string> onSuccess, Action<string> onFail) {
            var baseQuery = "https://api.twitch.tv/kraken/search/streams?query=";
            var limitQuery = "&limit=" + limit.ToString(); 
            var url = baseQuery + gameTitle + limitQuery + limit;

            StartCoroutine(WebRequestRoutine(url, onSuccess, onFail));
        }

        private IEnumerator WebRequestRoutine(string url, Action<string> onSuccess, Action<string> onFail) {
            Debug.Log("Get data from Twitch");
            //create webrequest
            UnityWebRequest www = UnityWebRequest.Get(url);

            //Add headers
            www.SetRequestHeader("Client-ID", clientId);
            www.SetRequestHeader("Accept", "application/vnd.twitchtv.v5+json");

            //wait for response
            yield return www.Send();

            if (www.isError) {
                Debug.Log("Error: " + www.error);
                if (onFail != null) onFail.Invoke(www.error);
            } else {
                Debug.Log("Got: " + www.downloadHandler.text);
                if (onSuccess != null) onSuccess.Invoke(www.downloadHandler.text);
            }
        }
    }
}
