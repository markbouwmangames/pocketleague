using UnityEngine;
using System.Collections.Generic;
using RLSApi.Net.Models;
using RLSApi.Net.Requests;
using RLSApi.Data;
using System;
using Newtonsoft.Json;

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

		/// <summary>
		/// Returns a single player
		/// </summary>
		/// <param name="platform">The platform the player is on</param>
		/// <param name="uniqueId">The UID of the player</param>
		/// <param name="onSuccess">Returns the player</param>
		/// <param name="onFail">Returns an error</param>
		public static void GetPlayer(RlsPlatform platform, string uniqueId, Action<Player> onSuccess, Action<Error> onFail) {
			var postfix = "player?unique_id=" + Uri.EscapeDataString(uniqueId) + "&platform_id=" + ((int)platform);
			Get<Player>(postfix, onSuccess, onFail);
		}
		
		public static void GetPlayers(IEnumerable<PlayerBatchRequest> players, Action<Player[]> onSuccess, Action<Error> onFail) {
			int count = 0;
			var enumerator = players.GetEnumerator();
			while (enumerator.MoveNext()) {
				count++;
			}
			if (count > 10) {
				throw new ArgumentException("You are trying to request too many players, the maximum is 10.");
			}

			var postfix = "player/batch";
			var requestData = JsonConvert.SerializeObject(players, Formatting.None);
			Post<Player[]>(postfix, requestData, onSuccess, onFail);
		}

		public static void GetLeaderboardRanked(RlsPlaylistRanked playlistRanked, Action<Player[]> onSuccess, Action<Error> onFail) {
			var postfix = "leaderboard/ranked?playlist_id=" + ((int)playlistRanked);
			Get<Player[]>(postfix, onSuccess, onFail);
		}

		public static void GetLeaderboardStats(RlsStatType statType, Action<Player[]> onSuccess, Action<Error> onFail) {
			var postfix = "leaderboard/stat?type=" + statType.ToString().ToLower();
			Get<Player[]>(postfix, onSuccess, onFail);
		}

		public static void SearchPlayer(string displayName, int page, Action<PlayerSearchPage> onSuccess, Action<Error> onFail) {
			var postfix = "search/players?display_name=";
			var postfix2 = "&page=";
			var userName = Uri.EscapeDataString(displayName);
			var totalPostfix = postfix + userName + postfix2 + page.ToString();
			Get<PlayerSearchPage>(totalPostfix, onSuccess, onFail);
		}

		public static void GetDivisionBreakdown(RlsPlaylistRanked playlistRanked, Action<List<List<int[]>>> onSuccess, Action<Error> onFail) {
			var url = "https://api.rocketleaguestats.com/web/graph/global/rating-breakdown?playlistId=" + ((int)(playlistRanked));
			Get<List<List<int[]>>>(url, onSuccess, onFail, true);
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

		private static void Get<T>(string urlPostfix, Action<T> onSuccess, Action<Error> onFail, bool urlOverride = false) {
			api.Get(urlPostfix, urlOverride, (data) => {
				//success
				var result = JsonConvert.DeserializeObject<T>(data);
				if (onSuccess != null) onSuccess.Invoke(result);
			}, (error) => {
				//error
				if (onFail != null) onFail.Invoke(error);
			});
		}

		private static void Post<T>(string urlPostfix, string postData, Action<T> onSuccess, Action<Error> onFail, bool urlOverride = false) {
			api.Post(urlPostfix, urlOverride, postData, (data) => {
				//success
				var result = JsonConvert.DeserializeObject<T>(data);
				if (onSuccess != null) onSuccess.Invoke(result);
			}, (error) => {
				//error
				if (onFail != null) onFail.Invoke(error);
			});
		}
		#endregion
	}
}
