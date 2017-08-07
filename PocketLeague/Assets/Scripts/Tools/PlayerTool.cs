using RLSApi.Net.Models;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using RLSApi.Data;
using System;
using RLSApi.Util;
using RLSApi;

public class PlayerTool : MonoBehaviour {
	private static PlayerTool _instance;
	void Awake() { _instance = this; }

	public static void GetPlayer(RlsPlatform platform, string uniqueId, Action<Player> onSuccess, Action<Error> onFail) {
		GetPlayer(new PlayerReferenceData(platform, uniqueId, uniqueId), onSuccess, onFail);
	}

	public static void GetPlayer(PlayerReferenceData playerReferenceData, Action<Player> onSuccess, Action<Error> onFail) {
		var database = FindObjectOfType<PlayerDatabase>();
		var player = database.GetStoredPlayer(playerReferenceData);

		if (player != null) {
			var difference = TimeUtil.Difference(DateTimeOffset.UtcNow, player.NextUpdateAt);
			if (difference > 0) {
				onSuccess.Invoke(player);
				return;
			}
		}

		RLSClient.GetPlayer(playerReferenceData.Platform, playerReferenceData.UniqueId, (data) => {
			database.StoreTempPlayer(data);
			onSuccess.Invoke(data);
		}, onFail);
	}

	public static void LoadAvatar(RawImage avatarIcon, Sprite defaultIcon, Player player) {
		_instance.LoadAvatarInstance(avatarIcon, defaultIcon, player);
	}

	private void LoadAvatarInstance(RawImage avatarIcon, Sprite defaultIcon, Player player) {
		var database = FindObjectOfType<PlayerDatabase>();

		var avatar = database.GetAvatar(player.Convert());

		if(avatar != null) {
			avatarIcon.texture = avatar;
			return;
		}

		avatarIcon.texture = defaultIcon.texture;

		if (!string.IsNullOrEmpty(player.Avatar)) {
			StartCoroutine(LoadAvatarRoutine(avatarIcon, defaultIcon, player));
		}
	}

	private IEnumerator LoadAvatarRoutine(RawImage avatarIcon, Sprite defaultIcon, Player player) {
		UnityWebRequest www = UnityWebRequest.GetTexture(player.Avatar);
		yield return www.Send();

		if (www.isError == false && www.responseCode >= 200 && www.responseCode <= 299) {
			var texture = DownloadHandlerTexture.GetContent(www);
			if(avatarIcon != null) avatarIcon.texture = texture;
			var database = FindObjectOfType<PlayerDatabase>();
			database.UpdateAvatar(player.Convert(), texture);
		} else {
			avatarIcon.texture = defaultIcon.texture;
		}
	}
}

public static class PlayerToolExtensions {
	public static Player[] SortbyName(this Player[] players, string original) {
		var comparer = new LevenshteinComparer(original);
		Array.Sort(players, comparer);
		return players;
	}

	public class LevenshteinComparer : IComparer {
		private string _original;

		public LevenshteinComparer(string original) {
			_original = original;
		}

		public int Compare(object a, object b) {
			var strA = ((Player)a).DisplayName;
			var strB = ((Player)b).DisplayName;

			var lDistA = LevenshteinDistance(strA, _original);
			var lDistB = LevenshteinDistance(strB, _original);

			if (lDistA == lDistB)
				return 0;
			if (lDistA < lDistB)
				return -1;
			return 1;
		}
	}

	private static int LevenshteinDistance(string s, string t) {
		int n = s.Length;
		int m = t.Length;
		int[,] d = new int[n + 1, m + 1];
		if (n == 0) {
			return m;
		}
		if (m == 0) {
			return n;
		}
		for (int i = 0; i <= n; d[i, 0] = i++)
			;
		for (int j = 0; j <= m; d[0, j] = j++)
			;
		for (int i = 1; i <= n; i++) {
			for (int j = 1; j <= m; j++) {
				int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
				d[i, j] = Math.Min(
					Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
					d[i - 1, j - 1] + cost);
			}
		}
		return d[n, m];
	}

	public static PlayerReferenceData Convert(this Player player) {
		var platform = PlatformTool.GetPlatformData(player.Platform).Platform;
		return new PlayerReferenceData(platform, player.UniqueId, player.DisplayName);
	}

	public static Dictionary<RlsPlaylistRanked, PlayerRank> MostRecentSeason(this Player player) {
		var rankedSeasons = player.RankedSeasons;

		var seasons = new RlsSeason[rankedSeasons.Count];
		rankedSeasons.Keys.CopyTo(seasons, 0);

		var latestSeason = seasons[seasons.Length - 1];
		return rankedSeasons[latestSeason];
	}

	public static Dictionary<RlsPlaylistRanked, PlayerRank> CurrentSeason(this Player player) {
		Dictionary<RlsPlaylistRanked, PlayerRank> output = null;
		if(player.RankedSeasons.TryGetValue(Constants.LatestSeason, out output)) {
			return output;
		} else {
			return null;
		}
	}
}