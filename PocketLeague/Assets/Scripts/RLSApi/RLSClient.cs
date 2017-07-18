using UnityEngine;
using RLSApi.Net.Models;
using RLSApi.Data;
using System;

namespace RLSApi {
	public class RLSClient {
		/// <summary>
		/// Initial values used for the API
		/// </summary>
		private static readonly string url = "https://api.rocketleaguestats.com/v1/";
		private static readonly string authKey = "ZXT5OU2S4B53F0GQ3UZHL7620K1MBV9G";
		private static readonly bool debug = true;

		/// <summary>
		/// Returns all tiers from the current season
		/// </summary>
		/// <param name="onSuccess">Returns a list of tiers</param>
		/// <param name="onFail">Returns an error</param>
		public static void GetTiers(Action<Tier[]> onSuccess, Action<Error> onFail) {
			var postfix = "data/tiers";
			Get<Tier[]>(postfix, onSuccess, onFail);
		}

		/// <summary>
		/// Returns all tiers from a selected season
		/// </summary>
		/// <param name="season">The <see cref="RlsSeason"/> you want tier data from</param>
		/// <param name="onSuccess">Returns a list of tiers</param>
		/// <param name="onFail">Returns an error</param>
		public static void GetTiers(RlsSeason season, Action<Tier[]> onSuccess, Action<Error> onFail) {
			var postfix = "data/tiers/" + ((int)season);
			Get<Tier[]>(postfix, onSuccess, onFail);
		}

		/// <summary>
		/// Returns all platforms
		/// </summary>
		/// <param name="onSuccess">Returns a list of platforms</param>
		/// <param name="onFail">Returns an error</param>
		public static void GetPlatforms(Action<Platform[]> onSuccess, Action<Error> onFail) {
			var postfix = "data/platforms";
			Get<Platform[]>(postfix, onSuccess, onFail);
		}

		/// <summary>
		/// Returns all seaons until now
		/// </summary>
		/// <param name="onSuccess">Returns a list of seasons, the last one being the current season</param>
		/// <param name="onFail">Returns an error</param>
		public static void GetSeasons(Action<Season[]> onSuccess, Action<Error> onFail) {
			var postfix = "data/seasons";
			Get<Season[]>(postfix, onSuccess, onFail);
		}

		/// <summary>
		/// Returns all playlists
		/// </summary>
		/// <param name="onSuccess">Returns a list of playlists</param>
		/// <param name="onFail">Returns an error</param>
		public static void GetPlaylists(Action<Playlist[]> onSuccess, Action<Error> onFail) {
			var postfix = "data/playlists";
			Get<Playlist[]>(postfix, onSuccess, onFail);
		}


		public static void GetPlayer(RlsPlatform platform, string uniqueId, Action<Player> onSuccess, Action<Error> onFail) {
			var postfix = "player?unique_id=" + Uri.EscapeDataString(uniqueId) + "&platform_id=" + ((int)platform);
			Get<Player>(postfix, onSuccess, onFail);
		}

		/// <summary>
		/// From here are the internal workings of the RLSClient
		/// </summary>
		#region INTERNAL
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

		private static void Get<T>(string urlPostfix, Action<T> onSuccess, Action<Error> onFail) {
			api.Get(urlPostfix, (data) => {
				//success
				var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data);
				if (onSuccess != null) onSuccess.Invoke(result);
			}, (error) => {
				//error
				if (onFail != null) onFail.Invoke(error);
			});
		}
		#endregion
	}
}
