using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Twitch.Net {
    public class TwitchApiRequester : MonoBehaviour {
        private string _clientId;
        private bool _debug;

        public void Init(string clientId, bool debug) {
            _clientId = clientId;
            _debug = debug;
        }

        public void GetTrendingClips(string gameTitle, int limit, Action<string> onSuccess, Action<string> onFail) {
            var baseQuery = "https://api.twitch.tv/kraken/search/streams?query=";
            var limitQuery = "&limit=" + limit.ToString();
            var url = baseQuery + gameTitle + limitQuery;

            StartCoroutine(WebRequestRoutine(url, onSuccess, onFail));
        }

        private IEnumerator WebRequestRoutine(string url, Action<string> onSuccess, Action<string> onFail) {
            //create webrequest
            UnityWebRequest www = UnityWebRequest.Get(url);

            //Add headers
            www.SetRequestHeader("Client-ID", _clientId);
            www.SetRequestHeader("Accept", "application/vnd.twitchtv.v5+json");

            //wait for response
            if (_debug) Debug.Log("GET data from " + url);
            yield return www.Send();

            if (!www.isError) {
                if (_debug) Debug.Log("GOT data from " + url + ", data: " + www.downloadHandler.text);
                if (onSuccess != null) onSuccess.Invoke(www.downloadHandler.text);
            } else {
                if (_debug) Debug.LogError("GOT error from " + url + ", error: " + www.error);
                if (onFail != null) onFail.Invoke(www.error);
            }
        }
    }
}
