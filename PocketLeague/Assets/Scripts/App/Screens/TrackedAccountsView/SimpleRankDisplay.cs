using UnityEngine;
using RLSApi.Data;
using System.Collections.Generic;
using RLSApi.Net.Models;
using UnityEngine.UI;
using System;

public class SimpleRankDisplay : MonoBehaviour {
	[SerializeField]
	private RectTransform _simplePlaylistRankTemplate;
	
	void Awake() {
		_simplePlaylistRankTemplate.gameObject.SetActive(false);
	}

	public void Set(RlsSeason season, Dictionary<RlsPlaylistRanked, PlayerRank> seasonData) {
		var path = "Data/Seasons/Season" + ((int)(season));
		var selectedSeason = Resources.Load<SeasonData>(path);

		if(seasonData == null) {
			seasonData = new Dictionary<RlsPlaylistRanked, PlayerRank>();
		}

		var playlists = Enum.GetValues(typeof(RlsPlaylistRanked));
		for (var i = 0; i < playlists.Length; i++) {
			var playlist = (RlsPlaylistRanked)playlists.GetValue(i);

			if(seasonData.ContainsKey(playlist) == false) {
				seasonData.Add(playlist, new PlayerRank());
			}

			var playerRank = seasonData[playlist];
			CreatePlaylistRank(selectedSeason, playerRank);
		}
	}

	private void CreatePlaylistRank(SeasonData selectedSeason, PlayerRank playerRank) {
		var simplePlaylistRank = UITool.CreateField<RectTransform>(_simplePlaylistRankTemplate.gameObject);

		var child = simplePlaylistRank.Find("Icon");
		var image = child.GetComponent<Image>();

		var sprite = selectedSeason.Ranks[0].Icon;
		var tier = playerRank.Tier;

		if (tier != null) {
			var index = tier.Value;
			sprite = selectedSeason.Ranks[index].Icon;
		}
		image.sprite = sprite;
	}
}
