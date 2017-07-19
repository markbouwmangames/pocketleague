using UnityEngine;
using RLSApi.Net.Models;
using RLSApi.Data;

public class RankView : PlayerViewChild {
	[SerializeField]
	private SeasonSelector _seasonSelector;

	public override void Set(Player player) {
		var rankedSeasons = player.RankedSeasons;

		var seasons = new RlsSeason[rankedSeasons.Count];
		rankedSeasons.Keys.CopyTo(seasons, 0);

		_seasonSelector.SetSeasonButtons(seasons);
	}

	public void SetSeason(RlsSeason season) {

	}
}
