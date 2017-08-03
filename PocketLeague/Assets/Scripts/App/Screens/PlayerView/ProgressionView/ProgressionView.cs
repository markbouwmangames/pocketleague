using RLSApi.Net.Models;
using UnityEngine;
using System;
using RLSApi.Data;
using System.Collections.Generic;

public class ProgressionView : PlayerViewChild {
	[SerializeField]
	private ProgressionBar _progressionBarTemplate;

	private Dictionary<RlsPlaylistRanked, ProgressionBar> _progressionBars = new Dictionary<RlsPlaylistRanked, ProgressionBar>();
	private SeasonData _currentSeason;

	void Awake() {
		var path = "Data/Seasons/Season" + ((int)(Constants.LatestSeason));
		_currentSeason = Resources.Load<SeasonData>(path);

		_progressionBarTemplate.gameObject.SetActive(false);

		var rankedPlaylists = Enum.GetValues(typeof(RlsPlaylistRanked));
		CreateProgressionBars(rankedPlaylists);
	}

	private void CreateProgressionBars(Array playlists) {
		for(var i = 0; i< playlists.Length; i++) {
			var playlist = (RlsPlaylistRanked)(playlists.GetValue(i));
			var progressionBar = UITool.CreateField<ProgressionBar>(_progressionBarTemplate);
			_progressionBars.Add(playlist, progressionBar);
		}
	}

	public override void Set(Player player) {
		var database = FindObjectOfType<DivisionBreakdownDatabase>();
		bool hasData = database.HasDivisionData();

		gameObject.SetActive(hasData);
		if (!hasData) return;

		var currentSeason = player.CurrentSeason();
		
		foreach(var kvp in currentSeason) {
			var playlist = kvp.Key;
			var rank = kvp.Value;

			var progressionBar = _progressionBars[playlist];
			progressionBar.Set(playlist, rank, _currentSeason);
		}
	}
}
