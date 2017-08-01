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
	[SerializeField]
	private Text _rankviewTitle;

	private Dictionary<RlsSeason, Dictionary<RlsPlaylistRanked, PlayerRank>> _rankedSeasons;

	void Awake() {
		_seasonSelector.OnSeasonChanged += OnSeasonChanged;
	}

	public override void Set(Player player) {
		_rankedSeasons = player.RankedSeasons;

		var seasons = new RlsSeason[_rankedSeasons.Count];
		_rankedSeasons.Keys.CopyTo(seasons, 0);

		if (_seasonSelector != null) {
			_seasonSelector.SetSeasonButtons(seasons);
		}

		var latest = seasons[seasons.Length - 1];
		SetSeason(latest);
	}

	private void OnSeasonChanged(RlsSeason value) {
		SetSeason(value);
	}

	public void SetSeason(RlsSeason season) {
		var seasonData = _rankedSeasons[season];
		_rankDisplay.Set(season, seasonData);
	}
}
