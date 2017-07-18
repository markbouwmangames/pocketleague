using UnityEngine;
using RLSApi.Models;
using System;

namespace RLSApi {
	public class RLSClient {
		private static readonly string url = "https://api.rocketleaguestats.com/v1/";
		private static readonly string authKey = "ZXT5OU2S4B53F0GQ3UZHL7620K1MBV9G";
		private static readonly bool debug = true;

		private static RLSApiRequester _rlsApiRequester;

		private static RLSApiRequester api {
			get {
				if (_rlsApiRequester == null) {
					//create gameObject
					var go = new GameObject("RLSApi");
					GameObject.DontDestroyOnLoad(go);

					//add api
					_rlsApiRequester = go.AddComponent<RLSApiRequester>();
					_rlsApiRequester.Init(url, authKey, debug);
				}

				return _rlsApiRequester;
			}
		}

		public static void GetTiers(Action<Tier[]> onSuccess, Action<Error> onFail) {
			GetArray<Tier>("data/tiers", onSuccess, onFail);
		}

		public static void Ba() {

		}

		private static void Get<T>(string urlPostfix, Action<T> onSuccess, Action<Error> onFail) {
			api.Get(urlPostfix, (data) => {
				//success
				var result = JsonUtility.FromJson<T>(data);
				if (onSuccess != null) onSuccess.Invoke(result);
			}, (error) => {
				//error
				if (onFail != null) onFail.Invoke(error);
			});
		}

		private static void GetArray<T>(string urlPostfix, Action<T[]> onSuccess, Action<Error> onFail) {
			api.Get(urlPostfix, (data) => {
				//success
				var result = FromJsonArray<T>(data);
				if (onSuccess != null) onSuccess.Invoke(result);
			}, (error) => {
				//error
				if (onFail != null) onFail.Invoke(error);
			});
		}

		private static T[] FromJsonArray<T>(string json) {
			json = "{\"Items\":" + json + "}";
			Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
			return wrapper.Items;
		}

		[Serializable]
		private class Wrapper<T> {
			public T[] Items = null;
		}
	}
}
