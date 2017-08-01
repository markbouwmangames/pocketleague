using System;
using System.Collections;
using RLSApi;
using RLSApi.Data;
using RLSApi.Net.Models;
using UnityEngine;
using System.Collections.Generic;

public class LeaderboardsView : BaseUpdateView {

	private RlsPlaylistRanked? _currentPlaylist;
	private RlsStatType? _currentStatType;

	private Dictionary<RlsPlaylistRanked, Player[]> _playerRankedLookup = new Dictionary<RlsPlaylistRanked, Player[]>();
	private Dictionary<RlsPlaylistRanked, Player[]> _playerUnrankedLookup = new Dictionary<RlsPlaylistRanked, Player[]>();

	private LeaderboardPlayerView _leaderboardPlayerViewTemplate;

	protected override void Init() {
		base.Init();
	}

	protected override void OpenView() {
		base.OpenView();
	}

	protected override void CloseView() {
		base.CloseView();
	}

	protected override void UpdateView(Action onComplete = null) {
		//load current player data
	}

	public void LoadRankedPlaylist(RlsPlaylistRanked playlist) {

	}

	public void LoadUnrankedPlaylist(RlsStatType statType) {

	}

	private IEnumerator LoadViewData(Action onComplete = null) {
	//	if (onComplete == null) Loader.OnLoadStart();

		yield return null;

		var playlists = Enum.GetValues(typeof(RlsPlaylistRanked));

		for(var i = 0; i < playlists.Length; i++) {

		}

		/*RLSClient.GetLeaderboardRanked(RLSApi.Data.RlsPlaylistRanked.Duel, (players) => {
			//succes
			if (onComplete != null) onComplete.Invoke();
			else Loader.OnLoadEnd();

			SetPlayers(players);
		}, (error) => {
			//error
			Debug.LogWarning("TODO: IMPLEMENT ERROR HANDLING");
		});*/
	}
}
