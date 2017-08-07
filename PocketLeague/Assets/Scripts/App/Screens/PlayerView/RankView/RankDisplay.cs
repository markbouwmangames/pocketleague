using RLSApi.Net.Models;
using System.Collections.Generic;
using UnityEngine;
using RLSApi.Data;
using System;

public class RankDisplay : MonoBehaviour {
	[SerializeField]
	private PlaylistRankDisplay _playlistRankDisplayTemplate;

	private List<PlaylistRankDisplay> _playlistRankDisplays = new List<PlaylistRankDisplay>();

	private SeasonData _selectedSeason;

	void Awake() {
		_playlistRankDisplayTemplate.gameObject.SetActive(false);

		for (int i = 0; i < 4; i++) {
			var prd = UITool.CreateField<PlaylistRankDisplay>(_playlistRankDisplayTemplate.gameObject);
			_playlistRankDisplays.Add(prd);
		}
	}

	public void Set(RlsSeason season, Dictionary<RlsPlaylistRanked, PlayerRank> seasonData) {
		var path = "Data/Seasons/Season" + ((int)(season));
		_selectedSeason = Resources.Load<SeasonData>(path);


		if (seasonData == null) {
			seasonData = new Dictionary<RlsPlaylistRanked, PlayerRank>();
		}


		var playlists = Enum.GetValues(typeof(RlsPlaylistRanked));
		for (var i = 0; i < playlists.Length; i++) {
			var playlistRankDisplay = _playlistRankDisplays[i];
			var playlist = (RlsPlaylistRanked)playlists.GetValue(i);

			if (seasonData.ContainsKey(playlist) == false) {
				seasonData.Add(playlist, new PlayerRank());
			}

			playlistRankDisplay.Set(_selectedSeason, playlist, seasonData[playlist]);
		}
	}
}
