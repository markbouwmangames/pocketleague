using RLSApi.Net.Models;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using RLSApi.Data;

public class PlayerTool : MonoBehaviour {
	private static PlayerTool _instance;
	void Awake() { _instance = this; }

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
	public static PlayerReferenceData Convert(this Player player) {
		var platform = PlatformTool.GetPlatform(player.Platform).Platform;
		return new PlayerReferenceData(platform, player.UniqueId, player.DisplayName);
	}

	public static Dictionary<RlsPlaylistRanked, PlayerRank> CurrentSeason(this Player player) {
		var rankedSeasons = player.RankedSeasons;

		var seasons = new RlsSeason[rankedSeasons.Count];
		rankedSeasons.Keys.CopyTo(seasons, 0);

		var latestSeason = seasons[seasons.Length - 1];
		return rankedSeasons[latestSeason];
	}
}