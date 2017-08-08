using UnityEngine;
using RLSApi.Net.Models;
using RLSApi.Data;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class RankView : PlayerViewChild {
	[SerializeField]
	private SeasonSelector _seasonSelector;
	[SerializeField]
	private RankDisplay _rankDisplay;

	private Dictionary<RlsSeason, Dictionary<RlsPlaylistRanked, PlayerRank>> _rankedSeasons;

	void Awake() {
		_seasonSelector.OnSeasonChanged += OnSeasonChanged;
	}

	public override void Set(Player player) {
		_rankedSeasons = player.RankedSeasons;

		RlsSeason[] seasons = null;

		if (_rankedSeasons.ContainsKey(Constants.LatestSeason) == false) {
			var temp = new RlsSeason[_rankedSeasons.Count];
			_rankedSeasons.Keys.CopyTo(temp, 0);

			seasons = new RlsSeason[_rankedSeasons.Count + 1];
			seasons[0] = Constants.LatestSeason;
			for (int i = 0; i < _rankedSeasons.Count; i++) {
				seasons[i + 1] = temp[i];
			}
		} else {
			seasons = new RlsSeason[_rankedSeasons.Count];
			_rankedSeasons.Keys.CopyTo(seasons, 0);
		}

		if (_seasonSelector != null) {
			_seasonSelector.SetSeasonButtons(seasons);
		}

		var latest = Constants.LatestSeason;
		SetSeason(latest);
	}

	private void OnSeasonChanged(RlsSeason value) {
		SetSeason(value);
	}

	public void SetSeason(RlsSeason season) {
		Dictionary<RlsPlaylistRanked, PlayerRank> seasonData = null;
		_rankedSeasons.TryGetValue(season, out seasonData);
		_rankDisplay.Set(season, seasonData);
	}
}
