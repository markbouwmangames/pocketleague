using System;
using RLSApi;
using RLSApi.Data;
using RLSApi.Net.Models;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class LeaderboardsView : BaseUpdateView {

	private RlsPlaylistRanked? _currentPlaylist;
	private RlsStatType? _currentStatType;

	private Dictionary<RlsPlaylistRanked, Player[]> _playerRankedLookup = new Dictionary<RlsPlaylistRanked, Player[]>();
	private Dictionary<RlsStatType, Player[]> _playerUnrankedLookup = new Dictionary<RlsStatType, Player[]>();

	[SerializeField]
	private LeaderboardPlayerView _leaderboardPlayerViewTemplate;
	[SerializeField]
	private LayoutElement _contentHolder; 
	[SerializeField]
	private LeaderboardViewSelector _leaderboardViewSelector;

	private List<LeaderboardPlayerView> _playerViews = new List<LeaderboardPlayerView>();

	protected override void Init() {
		_leaderboardPlayerViewTemplate.gameObject.SetActive(false);
		base.Init();
	}

	protected override void OpenView() {
		base.OpenView();
		_leaderboardViewSelector.Open();
	}

	protected override void CloseView() {
		base.CloseView();
	}

	protected override void UpdateView(Action onComplete = null) {
		//load current player data
		/*_playerRankedLookup.Clear();
		_playerUnrankedLookup.Clear();

		if (_currentPlaylist != null) {
			ShowLeaderboard(_currentPlaylist.Value);
		}

		if (_currentStatType != null) {
			ShowLeaderboard(_currentStatType.Value);
		}*/
	}

	public void ShowLeaderboard(RlsPlaylistRanked playlist) {
		Debug.Log("RlsPlaylistRanked");
		_currentPlaylist = playlist;
		_currentStatType = null;
		ResetView();

		if (_playerRankedLookup.ContainsKey(playlist)) {
			SetView(playlist, _playerRankedLookup[playlist]);
		} else {
			LoadPlaylist(playlist);
		}
	}

	public void ShowLeaderboard(RlsStatType statType) {
		Debug.Log("RlsStatType");
		_currentPlaylist = null;
		_currentStatType = statType;
		ResetView();

		if (_playerUnrankedLookup.ContainsKey(statType)) {
			SetView(statType, _playerUnrankedLookup[statType]);
		} else {
			LoadPlaylist(statType);
		}
	}

	private void SetView(RlsPlaylistRanked playlist, Player[] players) {
		for (var i = 0; i < players.Length; i++) {
			var player = players[i];
			CreateLeaderboardPlayerView(i + 1, GetStat(playlist, player), player);
		}
		SetView(players.Length);
	}

	private void SetView(RlsStatType statType, Player[] players) {
		for (var i = 0; i < players.Length; i++) {
			var player = players[i];
			CreateLeaderboardPlayerView(i + 1, GetStat(statType, player), player);
		}
		SetView(players.Length);
	}

	private void LoadPlaylist(RlsPlaylistRanked playlist) {
		RLSClient.GetLeaderboardRanked(playlist, (players) => {
			//succes
			SetLookup(playlist, players);

			if (_currentPlaylist == null || _currentPlaylist != playlist) return;

			SetView(playlist, players);
		}, (error) => {
			//error
			Debug.LogWarning("TODO: IMPLEMENT ERROR HANDLING");
		});
	}

	private void LoadPlaylist(RlsStatType statType) {
		RLSClient.GetLeaderboardStats(statType, (players) => {
			//succes
			SetLookup(statType, players);
			if (_currentStatType == null || _currentStatType != statType) return;
			SetView(statType, players);

		}, (error) => {
			//error
			Debug.LogWarning("TODO: IMPLEMENT ERROR HANDLING");
		});
	}

	private void CreateLeaderboardPlayerView(int rank, int stat, Player player) {
		LeaderboardPlayerView leaderboardPlayerView = null;
		if ((rank - 1) < _playerViews.Count) {
			leaderboardPlayerView = _playerViews[rank - 1];
			leaderboardPlayerView.gameObject.SetActive(true);
		} else {
			leaderboardPlayerView = UITool.CreateField<LeaderboardPlayerView>(_leaderboardPlayerViewTemplate);
			_playerViews.Add(leaderboardPlayerView);
		}

		leaderboardPlayerView.Set(rank, stat, player);
	}

	private void SetLookup(RlsPlaylistRanked playlist, Player[] players) {
		if (_playerRankedLookup.ContainsKey(playlist) == false) {
			_playerRankedLookup.Add(playlist, players);
		} else {
			_playerRankedLookup[playlist] = players;
		}
	}
	
	private void SetLookup(RlsStatType statType, Player[] players) {
		if (_playerUnrankedLookup.ContainsKey(statType) == false) {
			_playerUnrankedLookup.Add(statType, players);
		} else {
			_playerUnrankedLookup[statType] = players;
		}
	}

	private int GetStat(RlsPlaylistRanked playlist, Player player) {
		var rankedSeasons = player.RankedSeasons;
		var seasons = new RlsSeason[rankedSeasons.Count];
		rankedSeasons.Keys.CopyTo(seasons, 0);
		var latestSeason = seasons[seasons.Length - 1];
		var stat = player.RankedSeasons[latestSeason][playlist].RankPoints;
		return stat;
	}

	private int GetStat(RlsStatType statType, Player player) {
		switch (statType) {
			case RlsStatType.Wins:
				return player.Stats.Wins;
			case RlsStatType.Goals:
				return player.Stats.Goals;
			case RlsStatType.Mvps:
				return player.Stats.Mvps;
			case RlsStatType.Saves:
				return player.Stats.Saves;
			case RlsStatType.Shots:
				return player.Stats.Shots;
			case RlsStatType.Assists:
				return player.Stats.Assists;
		}
		return 0;
	}

	private void ResetView() {
		foreach (var v in _playerViews) {
			v.gameObject.SetActive(false);
		}
		_contentHolder.preferredHeight = 0;
		var rectTransform = _contentHolder.GetComponent<RectTransform>();
		var size = rectTransform.sizeDelta;
		size.y = 0;
		rectTransform.sizeDelta = size;
	}

	private void SetView(int numChildren) {
		_contentHolder.preferredHeight = numChildren * 100;
		var rectTransform = _contentHolder.GetComponent<RectTransform>();
		var size = rectTransform.sizeDelta;
		size.y = _contentHolder.preferredHeight;
		rectTransform.sizeDelta = size;
	}
}
