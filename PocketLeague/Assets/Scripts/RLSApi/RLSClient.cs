using UnityEngine;
using RLSApi.Models;
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
			GetArray<Tier>(postfix, onSuccess, onFail);
		}

		/// <summary>
		/// Returns all tiers from a selected season
		/// </summary>
		/// <param name="season">The <see cref="RlsSeason"/> you want tier data from</param>
		/// <param name="onSuccess">Returns a list of tiers</param>
		/// <param name="onFail">Returns an error</param>
		public static void GetTiers(RlsSeason season, Action<Tier[]> onSuccess, Action<Error> onFail) {
			var postfix = "data/tiers/" + ((int)season);
			GetArray<Tier>(postfix, onSuccess, onFail);
		}

		/// <summary>
		/// Returns all platforms
		/// </summary>
		/// <param name="onSuccess">Returns a list of platforms</param>
		/// <param name="onFail">Returns an error</param>
		public static void GetPlatforms(Action<Platform[]> onSuccess, Action<Error> onFail) {
			var postfix = "data/platforms";
			GetArray<Platform>(postfix, onSuccess, onFail);
		}

		/// <summary>
		/// Returns all seaons until now
		/// </summary>
		/// <param name="onSuccess">Returns a list of seasons, the last one being the current season</param>
		/// <param name="onFail">Returns an error</param>
		public static void GetSeasons(Action<Season[]> onSuccess, Action<Error> onFail) {
			var postfix = "data/seasons";
			GetArray<Season>(postfix, onSuccess, onFail);
		}

		/// <summary>
		/// Returns all playlists
		/// </summary>
		/// <param name="onSuccess">Returns a list of playlists</param>
		/// <param name="onFail">Returns an error</param>
		public static void GetPlaylists(Action<Playlist[]> onSuccess, Action<Error> onFail) {
			var postfix = "data/playlists";
			GetArray<Playlist>(postfix, onSuccess, onFail);
		}


		public static void GetPlayer(RlsPlatform platform, string uniqueId, Action<Player> onSuccess, Action<Error> onFail) {
			var postfix = "player?unique_id=" + Uri.EscapeDataString(uniqueId) + "&platform_id=" + ((int)platform);
			GetPlayer(postfix, onSuccess, onFail);
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
				var result = JsonUtility.FromJson<T>(data);
				if (onSuccess != null) onSuccess.Invoke(result);
			}, (error) => {
				//error
				if (onFail != null) onFail.Invoke(error);
			});
		}

		private static void GetPlayer(string urlPostfix, Action<Player> onSuccess, Action<Error> onFail) {
			api.Get(urlPostfix, (data) => {
				//success
				var result = FromPlayerData(data);
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
		
		public static Player FromPlayerData(string json) {
			var result = JsonUtility.FromJson<Player>(json);
			var rankedSeasonsJSON = GetRankedSeasonsJSON(json);
			var seasonData = GetSeasonData(rankedSeasonsJSON);

			foreach(var sd in seasonData) {
				var obj = SimpleJSON.JSON.Parse(sd);
				Debug.Log(obj[1]);
			}

			return result;
		}

		private static string GetRankedSeasonsJSON(string json) {
			var index = json.IndexOf("rankedSeasons");
			var substring = json.Substring(index);
			int numbrackets = 0;
			var startIndex = 0;
			var endIndex = 0;

			for (int i = 0; i < substring.Length; i++) {
				if (substring[i] == '{') {
					if (numbrackets == 0) startIndex = i;
					numbrackets++;
				}

				if (substring[i] == '}') {
					if (numbrackets == 1) {
						endIndex = i + 1;
						break;
					}

					numbrackets--;
				}

			}

			var substring2 = substring.Substring(startIndex, endIndex - startIndex);
			return substring2;
		}

		private static string[] GetSeasonData(string json) {
			var seasons = json.Split(new string[] { "}}," }, StringSplitOptions.None);
			for (int i = 0; i < seasons.Length; i++) {
				if (seasons[i].StartsWith("{") == false) {
					seasons[i] = "{" + seasons[i];
				}
				if (seasons[i].EndsWith("}}}") == false) {
					seasons[i] = seasons[i] + "}}}";
				}
			}

			return seasons;
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
		#endregion
	}
}
