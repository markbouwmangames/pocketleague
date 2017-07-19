using UnityEngine;
using System;
using System.Collections;
using RLSApi.Net.Models;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace RLSApi {
	public class RLSApiRequester : MonoBehaviour {
		private string _url;
		private string _authKey;
		private bool _debug;

		public void Init(string url, string authKey, bool debug) {
			//set initial values passed by RLSClient
			_url = url;
			_authKey = authKey;
			_debug = debug;
		}

		public void Get(string urlPosfix, Action<string> onSuccess, Action<Error> onFail) {
			//start request routine
			StartCoroutine(WebRequestRoutine(_url + urlPosfix, (data) => {
				//on succes callback
				onSuccess.Invoke(data);
			}, (data) => {
				//on fail callback
                var result = new Error() { Message = data };
				onFail.Invoke(result);
			}));
		}

		public void Post(string urlPosfix, string postData, Action<string> onSuccess, Action<Error> onFail) {
			//start post routine
			StartCoroutine(WebPostRoutine(_url + urlPosfix, postData, (data) => {
				//on succes callback
				onSuccess.Invoke(data);
			}, (data) => {
                //on fail callback
                var result = new Error() { Message = data };
				onFail.Invoke(result);
			}));
		}

		private IEnumerator WebRequestRoutine(string url, Action<string> onSuccess, Action<string> onFail) {
			//create webrequest
			UnityWebRequest www = UnityWebRequest.Get(url);
			www.SetRequestHeader("Authorization", _authKey);

			//wait for response
			if (_debug) Debug.Log("GET data from " + url);
			yield return www.Send();

			if (!www.isError) {
				if (_debug)	Debug.Log("GOT data from " + url + ", data: " + www.downloadHandler.text);
				onSuccess.Invoke(www.downloadHandler.text);
			} else { 
				if (_debug) Debug.LogError("GOT error from " + url + ", error: " + www.error);
				onFail.Invoke(www.error);
			}
		}

		private IEnumerator WebPostRoutine(string url, string postData, Action<string> onSuccess, Action<string> onFail) {
			//create webrequest

			var www = new UnityWebRequest(url, "POST");
			byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(postData);
			www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
			www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
			www.SetRequestHeader("Content-Type", "application/json");
			www.SetRequestHeader("Authorization", _authKey);

			//wait for response
			if (_debug) Debug.Log("POST data to " + url + ", data: " + postData);
			yield return www.Send();

			if (!www.isError) {
				if (_debug) Debug.Log("GOT data from " + url + ", data: " + www.downloadHandler.text);
				onSuccess.Invoke(www.downloadHandler.text);
			} else {
				if (_debug) Debug.LogError("GOT error from " + url + ", error: " + www.error);
				onFail.Invoke(www.error);
			}
		}
	}
}