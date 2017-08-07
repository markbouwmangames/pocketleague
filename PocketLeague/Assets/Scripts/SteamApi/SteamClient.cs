using Newtonsoft.Json;
using System;
using UnityEngine;
using SteamApi.Net;
using SteamApi.Net.Models;

namespace SteamApi {
	public class SteamClient {
		/// <summary>
		/// Initial values used for the API
		/// </summary>
		private static readonly string clientId = "BDED70AC0787EF3B3C5ED286EEB3CDD4";
		private static readonly bool debug = true;
		private static readonly bool throttleRequests = true;

		public static void ResolveVanityURL(string id, Action<SteamData> onSuccess, Action<string> onFail) {
			api.ResolveVanityURL(id, (data) => {
				var response = JsonConvert.DeserializeObject<SteamResponse>(data);
				if (onSuccess != null) onSuccess.Invoke(response.SteamData);
			}, onFail);
		}

		#region INTERNAL
		private static SteamApiRequester _steamApiRequester;
		private static SteamApiRequester api {
			get {
				if (_steamApiRequester == null) {
					//create gameObject
					var go = new GameObject("SteamAPI");
					GameObject.DontDestroyOnLoad(go);

					//add api
					_steamApiRequester = go.AddComponent<SteamApiRequester>();
					_steamApiRequester.Init(clientId, debug, throttleRequests);
				}

				return _steamApiRequester;
			}
		}
		#endregion
	}
}