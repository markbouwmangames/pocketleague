using System;
using RLSApi;
using RLSApi.Data;
using RLSApi.Net.Models;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using RLSApi.Util;

public class LeaderboardsView : BaseUpdateView {

	private RlsPlaylistRanked? _currentPlaylist;
	private RlsStatType? _currentStatType;

	private Dictionary<RlsPlaylistRanked, Player[]> _playerRankedLookup = new Dictionary<RlsPlaylistRanked, Player[]>();
	private Dictionary<RlsStatType, Player[]> _playerUnrankedLookup = new Dictionary<RlsStatType, Player[]>();

	private Dictionary<RlsPlaylistRanked, DateTimeOffset> _playerRankedLoadTime = new Dictionary<RlsPlaylistRanked, DateTimeOffset>();
	private Dictionary<RlsStatType, DateTimeOffset> _playerunRankedLoadTime = new Dictionary<RlsStatType, DateTimeOffset>();
	
	[SerializeField]
	private LeaderboardPlayerView _leaderboardPlayerViewTemplate;
	[SerializeField]
	private LayoutElement _contentHolder; 
	[SerializeField]
	private LeaderboardViewSelector _leaderboardViewSelector;

	private List<LeaderboardPlayerView> _playerViews = new List<LeaderboardPlayerView>();

	private Action _onComplete;

	protected override void Init() {
		_leaderboardPlayerViewTemplate.gameObject.SetActive(false);
		base.Init();
	}

	protected override void OpenView() {
		base.OpenView();
	}

	protected override void CloseView() {
		base.CloseView();
	}

	protected override void UpdateView(Action onComplete = null) {
		_onComplete = onComplete;

		if (onComplete == null) {
			Loader.OnLoadStart();
			_leaderboardViewSelector.Open();
		}  else {
			if (_currentPlaylist != null) {
				_playerRankedLookup.Remove(_currentPlaylist.Value);
				ShowLeaderboard(_currentPlaylist.Value);
			}

			if (_currentStatType != null) {
				_playerUnrankedLookup.Remove(_currentStatType.Value);
				ShowLeaderboard(_currentStatType.Value);
			}
		}
	}

	public void ShowLeaderboard(RlsPlaylistRanked playlist) {
		_currentPlaylist = playlist;
		_currentStatType = null;
		ResetView();

		if (_playerRankedLookup.ContainsKey(playlist)) {
			var time = _playerRankedLoadTime[playlist];
			var difference = TimeUtil.Difference(DateTimeOffset.UtcNow, time);
			if (difference > 0) {
				SetView(playlist, _playerRankedLookup[playlist]);
			} else {
				LoadPlaylist(playlist);
			}
		} else {
			LoadPlaylist(playlist);
		}
	}

	public void ShowLeaderboard(RlsStatType statType) {
		_currentPlaylist = null;
		_currentStatType = statType;
		ResetView();

		if (_playerUnrankedLookup.ContainsKey(statType)) {
			var time = _playerunRankedLoadTime[statType];
			var difference = TimeUtil.Difference(DateTimeOffset.UtcNow, time);
			if (difference > 0) {
				SetView(statType, _playerUnrankedLookup[statType]);
			} else {
				LoadPlaylist(statType);
			}
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
		var nextUpdateTime = (DateTimeOffset.UtcNow).AddMinutes(15);
		if (_playerRankedLoadTime.ContainsKey(playlist)) {
			_playerRankedLoadTime[playlist] = nextUpdateTime;
		} else {
			_playerRankedLoadTime.Add(playlist, nextUpdateTime);
		}

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
		var nextUpdateTime = (DateTimeOffset.UtcNow).AddMinutes(15);
		if (_playerunRankedLoadTime.ContainsKey(statType)) {
			_playerunRankedLoadTime[statType] = nextUpdateTime;
		} else {
			_playerunRankedLoadTime.Add(statType, nextUpdateTime);
		}

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
		var stat = player.CurrentSeason()[playlist].RankPoints;
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
	}

	private void SetView(int numChildren) {
		if(_onComplete != null) {
			_onComplete();
		} else {
			Loader.OnLoadEnd();
		}

		_contentHolder.preferredHeight = numChildren * 100;
		var rectTransform = _contentHolder.GetComponent<RectTransform>();
		var size = rectTransform.sizeDelta;
		size.y = _contentHolder.preferredHeight;
		rectTransform.sizeDelta = size;
	}
}
