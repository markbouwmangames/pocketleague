using RLSApi.Net.Models;
using UnityEngine;
using System.Collections.Generic;

public class StatsView : PlayerViewChild {
	[SerializeField]
	private StatDisplay _statDisplayTemplate;

	private List<StatDisplay> _statDisplays = new List<StatDisplay>();

	private SeasonData _selectedSeason;

	void Awake() {
		_statDisplayTemplate.gameObject.SetActive(false);

		for (int i = 0; i < 8; i++) {
			var statDisplay = CreateField();
			_statDisplays.Add(statDisplay);
		}
	}


	public override void Set(Player player) {
		var stats = player.Stats;

		_statDisplays[0].Set("WINS", stats.Wins);
		_statDisplays[1].Set("GOALS", stats.Goals);
		_statDisplays[2].Set("SHOTS", stats.Shots);
		_statDisplays[3].Set("ASSISTS", stats.Assists);
		_statDisplays[4].Set("SAVES", stats.Saves);
		_statDisplays[5].Set("SHOTACCURACY", stats.ShotAccuracy, "%");
		_statDisplays[6].Set("MVPS", stats.Mvps);
		_statDisplays[7].Set("MVPPERCENTAGE", stats.MvpPercentage);
	}

	private StatDisplay CreateField() {
		var newGO = GameObject.Instantiate(_statDisplayTemplate.gameObject);

		newGO.transform.SetParent(_statDisplayTemplate.transform.parent);
		newGO.transform.localScale = _statDisplayTemplate.transform.localScale;

		newGO.SetActive(true);
		return newGO.GetComponent<StatDisplay>();
	}
}
