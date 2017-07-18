using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using RLSApi.Models;

namespace RLSApi {
	public class RLSApiRequester : MonoBehaviour {
		private string _url;
		private string _authKey;
		private bool _debug;

		public void Init(string url, string authKey, bool debug) {
			_url = url;
			_authKey = authKey;
			_debug = debug;
		}

		public void Get(string urlPosfix, Action<string> onSuccess, Action<Error> onFail) {
			DoWWWRoutine(_url + urlPosfix, (data) => {
				//succes
				onSuccess.Invoke(data);
			}, (data) => {
				//fail
				var result = JsonUtility.FromJson<Error>(data);
				onFail.Invoke(result);
			});
		}

		private void DoWWWRoutine(string url, Action<string> onSuccess, Action<string> onFail) {
			//start the request routine
			StartCoroutine(WWWRoutine(url, onSuccess, onFail));
		}

		private IEnumerator WWWRoutine(string url, Action<string> onSuccess, Action<string> onFail) {
			//create auth headers
			var headerInfo = new Dictionary<string, string>();
			headerInfo.Add("Authorization", _authKey);

			if(_debug) {
				Debug.Log("GET from " + url);
			}

			//www request
			var www = new WWW(url, null, headerInfo);

			//wait for reply
			yield return www;

			//return outcome
			if (string.IsNullOrEmpty(www.error)) {
				if (_debug) {
					Debug.Log("GOT data from " + url + ", data: " + www.text);
				}
				onSuccess.Invoke(www.text);
			} else {
				if (_debug) {
					Debug.LogError("GOT error from " + url + ", error: " + www.error);
				}
				onFail.Invoke(www.error);
			}
		}
	}
}