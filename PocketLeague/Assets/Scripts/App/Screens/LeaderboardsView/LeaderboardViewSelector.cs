using UnityEngine;
using UnityEngine.UI;
using RLSApi.Data;
using System;

public class LeaderboardViewSelector : MonoBehaviour {
	[SerializeField]
	private LeaderboardsView _leaderboardsView;

	[SerializeField]
	private Toggle _rankedSelectToggle;
	[SerializeField]
	private Toggle _unrankedSelectToggle;

	[SerializeField]
	private GameObject _rankedToggles;
	[SerializeField]
	private GameObject _unrankedToggles;

	[SerializeField]
	private LeaderboardToggle _rankedLeaderboardToggleTemplate;
	[SerializeField]
	private LeaderboardToggle _unrankedLeaderboardToggleTemplate;

	private Toggle _rankedLeaderboardFirstToggle;
	private Toggle _unrankedLeaderboardFirstToggle;

	void Awake() {
		_rankedLeaderboardToggleTemplate.gameObject.SetActive(false);
		_unrankedLeaderboardToggleTemplate.gameObject.SetActive(false);
		
		CreateRankedToggles();
		CreateUnrankedToggles();

		SetupDefault();

		_rankedSelectToggle.onValueChanged.AddListener(OnRankedToggleClicked);
		_unrankedSelectToggle.onValueChanged.AddListener(OnUnrankedToggleClicked);
	}

	private void SetupDefault() {
		_rankedSelectToggle.isOn = true;
		_rankedLeaderboardFirstToggle.isOn = true;

		_rankedToggles.SetActive(true);
		_unrankedToggles.SetActive(false);
	}

	private void CreateRankedToggles() {
		var playlists = Enum.GetValues(typeof(RlsPlaylistRanked));
		for(var i = 0; i < playlists.Length; i++) {
			var playlist = (RlsPlaylistRanked)(playlists.GetValue(i));

			var newToggle = UITool.CreateField<LeaderboardToggle>(_rankedLeaderboardToggleTemplate);
			newToggle.SetText(playlist.ToString().ToUpper());
			newToggle.OnClick += () => { OnRankedToggleClick(playlist); };

			if(i == 0) {
				var toggle = newToggle.GetComponent<Toggle>();
				_rankedLeaderboardFirstToggle = toggle;
			}
		}
	}

	private void CreateUnrankedToggles() {
		var statTypes = Enum.GetValues(typeof(RlsStatType));
		for (var i = 0; i < statTypes.Length; i++) {
			var statType = (RlsStatType)(statTypes.GetValue(i));

			var newToggle = UITool.CreateField<LeaderboardToggle>(_unrankedLeaderboardToggleTemplate);
			newToggle.SetText(statType.ToString().ToUpper());
			newToggle.OnClick += () => { OnUnrankedToggleClick(statType); };

			if (i == 0) {
				var toggle = newToggle.GetComponent<Toggle>();
				_unrankedLeaderboardFirstToggle = toggle;
			}
		}
	}

	private void OnRankedToggleClick(RlsPlaylistRanked playlist) {
		_leaderboardsView.LoadRankedPlaylist(playlist);
	}

	private void OnUnrankedToggleClick(RlsStatType statType) {
		_leaderboardsView.LoadUnrankedPlaylist(statType);
	}

	private void OnRankedToggleClicked(bool value) {
		OnToggleClicked(_rankedToggles, _rankedLeaderboardFirstToggle, value);
	}

	private void OnUnrankedToggleClicked(bool value) {
		OnToggleClicked(_unrankedToggles, _unrankedLeaderboardFirstToggle, value);
	}

	private void OnToggleClicked(GameObject root, Toggle firstToggle, bool value) {
		if (root.activeInHierarchy && value == true) return;
		if (!root.activeInHierarchy && value != true) return;

		if (value) {
			if (firstToggle.isOn == false) {
				firstToggle.isOn = true;
			} else {
				firstToggle.onValueChanged.Invoke(true);
			}
		}
		root.SetActive(value);
	}

}
