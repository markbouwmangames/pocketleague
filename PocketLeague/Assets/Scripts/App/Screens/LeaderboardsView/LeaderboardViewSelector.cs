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

	private Toggle[] _rankedLeaderboardToggles;
	private Toggle[] _unrankedLeaderboardToggles;

	void Awake() {
		_rankedLeaderboardToggleTemplate.gameObject.SetActive(false);
		_unrankedLeaderboardToggleTemplate.gameObject.SetActive(false);
		
		CreateRankedToggles();
		CreateUnrankedToggles();

		SetupDefault();

		_rankedSelectToggle.onValueChanged.AddListener(OnRankedHeaderToggle);
		_unrankedSelectToggle.onValueChanged.AddListener(OnUnrankedHeaderToggle);
	}
	
	public void Open() {
		if (_rankedSelectToggle.isOn == false) {
			_rankedSelectToggle.isOn = true;
		} else {
			var toggle = _rankedLeaderboardToggles[0];
			if (toggle.isOn == false) toggle.isOn = true;
			else toggle.onValueChanged.Invoke(true);
		}
	}

	private void SetupDefault() {
		_rankedSelectToggle.isOn = true;
		_rankedLeaderboardToggles[0].isOn = true;

		_rankedToggles.SetActive(true);
		_unrankedToggles.SetActive(false);
	}

	private void CreateRankedToggles() {
		var playlists = Enum.GetValues(typeof(RlsPlaylistRanked));

		_rankedLeaderboardToggles = new Toggle[playlists.Length];

		for (var i = 0; i < playlists.Length; i++) {
			var playlist = (RlsPlaylistRanked)(playlists.GetValue(i));

			var newToggle = UITool.CreateField<LeaderboardToggle>(_rankedLeaderboardToggleTemplate);
			newToggle.SetText(playlist.ToString().ToUpper());
			newToggle.OnClick += () => { OnRankedToggleClick(playlist); };

			var toggle = newToggle.GetComponent<Toggle>();
			_rankedLeaderboardToggles[i] = toggle;

			if (i != 0) toggle.isOn = false;
		}
	}

	private void CreateUnrankedToggles() {
		var statTypes = Enum.GetValues(typeof(RlsStatType));
		_unrankedLeaderboardToggles = new Toggle[statTypes.Length];

		for (var i = 0; i < statTypes.Length; i++) {
			var statType = (RlsStatType)(statTypes.GetValue(i));

			var newToggle = UITool.CreateField<LeaderboardToggle>(_unrankedLeaderboardToggleTemplate);
			newToggle.SetText(statType.ToString().ToUpper());
			newToggle.OnClick += () => { OnUnrankedToggleClick(statType); };

			var toggle = newToggle.GetComponent<Toggle>();
			_unrankedLeaderboardToggles[i] = toggle;

			if (i != 0) toggle.isOn = false;
		}
	}


	private void OnRankedHeaderToggle(bool value) {
		OnHeaderToggleClicked(_rankedToggles, _rankedLeaderboardToggles, value);
	}

	private void OnUnrankedHeaderToggle(bool value) {
		OnHeaderToggleClicked(_unrankedToggles, _unrankedLeaderboardToggles, value);
	}

	private void OnRankedToggleClick(RlsPlaylistRanked playlist) {
		_leaderboardsView.ShowLeaderboard(playlist);
	}

	private void OnUnrankedToggleClick(RlsStatType statType) {
		_leaderboardsView.ShowLeaderboard(statType);
	}

	private void OnHeaderToggleClicked(GameObject root, Toggle[] subToggles, bool value) {
		if (root.activeInHierarchy && value == true) return;
		if (!root.activeInHierarchy && value != true) return;

		if (value) {
			if (subToggles[0].isOn == false) {
				subToggles[0].isOn = true;
			} else {
				subToggles[0].onValueChanged.Invoke(true);
			}

			for(int i = 1; i < subToggles.Length; i++) {
				subToggles[i].isOn = false;
			}
		}

		root.SetActive(value);
	}

}
