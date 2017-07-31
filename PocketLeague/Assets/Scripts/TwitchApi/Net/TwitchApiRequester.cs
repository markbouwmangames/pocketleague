using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Twitch.Net {
    public class TwitchApiRequester : MonoBehaviour {
		private struct WWWRequestData {
			public string Url;
			public Action<string> OnSuccess;
			public Action<string> OnFail;
		}

		private string _clientId;
        private bool _debug;
		private bool _throttleRequests;

		private Queue<WWWRequestData> _requestQueue = new Queue<WWWRequestData>();

		public void Init(string clientId, bool debug, bool throttleRequests) {
            _clientId = clientId;
            _debug = debug;
			_throttleRequests = throttleRequests;

			if (_throttleRequests) {
				StartCoroutine(ThrottleRequests());
			}
		}

		public void GetTrendingClips(string gameTitle, int limit, Action<string> onSuccess, Action<string> onFail) {
			var baseQuery = "https://api.twitch.tv/kraken/search/streams?query=";
			var limitQuery = "&limit=" + limit.ToString();
			var url = baseQuery + gameTitle + limitQuery;


			if (_throttleRequests) _requestQueue.Enqueue(new WWWRequestData() {
				Url = url,
				OnSuccess = onSuccess,
				OnFail = onFail
			});
			else DoWWW(url, onSuccess, onFail);
		}

		private void DoWWW(string url, Action<string> onSuccess, Action<string> onFail) {
			//start request routine
			StartCoroutine(WebRequestRoutine(url, (data) => {
				//on succes callback
				onSuccess.Invoke(data);
			}, (data) => {
				//on fail callback
				if (data.isError) onFail.Invoke(data.error);
				else onFail.Invoke(data.downloadHandler.text);
				
			}));
		}

		private IEnumerator ThrottleRequests() {
			while (true) {
				yield return null;
				//send request
				if (_requestQueue.Count > 0) {
					var request = _requestQueue.Dequeue();
					yield return StartCoroutine(WebRequestRoutine(request.Url, request.OnSuccess, (data) => {
						//server too busy, retry
						if (data.responseCode == 429) _requestQueue.Enqueue(request);
						else if (data.isError) request.OnFail(data.error);
						else request.OnFail(data.downloadHandler.text);
					}));
				}
			}
		}

        private IEnumerator WebRequestRoutine(string url, Action<string> onSuccess, Action<UnityWebRequest> onFail) {
            //create webrequest
            UnityWebRequest www = UnityWebRequest.Get(url);

            //Add headers
            www.SetRequestHeader("Client-ID", _clientId);
            www.SetRequestHeader("Accept", "application/vnd.twitchtv.v5+json");

            //wait for response
            if (_debug) Debug.Log("GET DATA from " + url);
            yield return www.Send();

            if (!www.isError && www.responseCode >= 200 && www.responseCode <= 299) {
                if (_debug) Debug.Log("GOT DATA from " + url + ", data: " + www.downloadHandler.text);
                if (onSuccess != null) onSuccess.Invoke(www.downloadHandler.text);
            } else {
                if (www.isError) {
					Debug.LogError("GOT WWW ERROR from " + url + ", error: " + www.error);
                    onFail.Invoke(www);
                } else {
                    Debug.LogError("GOT SERVER ERROR from " + url + ", error: " + www.downloadHandler.text);
                    onFail.Invoke(www);
                }
            }
        }
    }
}
