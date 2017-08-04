using UnityEngine;
using RLSApi.Net.Models;
using RLSApi;
using Twitch;
using Twitch.Net.Models;
using System.Collections.Generic;
using System;

public class HomeView : BaseUpdateView {
    [SerializeField]
    private PlayerQuickView _playerQuickView;

    [SerializeField]
    private PlaylistPopulationView _playlistView;

    [SerializeField]
    private TwitchView _twitchView;

    private PlayerReferenceData _mainAccount;
	private Dictionary<string, bool> _hasLoaded = new Dictionary<string, bool>();

    protected override void Init() {
        _hasLoaded.Add("GetPlayer", false);
		_hasLoaded.Add("GetPlaylists", false);
		_hasLoaded.Add("GetTrendingClips", false);

        _playerQuickView.OnClick += () => {
            var app = FindObjectOfType<App>();
            app.SetPlayerView(_mainAccount);
        };

		base.Init();
	}

    protected override void UpdateView(Action onComplete = null) {
        if (onComplete == null) Loader.OnLoadStart();

        _hasLoaded["GetPlayer"] = false;
		_hasLoaded["GetPlaylists"] = false;
		_hasLoaded["GetTrendingClips"] = false;

        //load player data
        RLSClient.GetPlayer(_mainAccount.Platform, _mainAccount.DisplayName, (player) => {
            //succes
            _hasLoaded["GetPlayer"] = true;
            if (HasLoadedAll && onComplete != null) onComplete.Invoke();
            else if (HasLoadedAll) Loader.OnLoadEnd();

            SetPlayer(player);
        }, (error) => {
            //error
            Debug.LogWarning("TODO: IMPLEMENT ERROR HANDLING");
        });

        //load playlist data
        RLSClient.GetPlaylists((data) => {
            //success
            _hasLoaded["GetPlaylists"] = true;
            if (HasLoadedAll && onComplete != null) onComplete.Invoke();
            else if (HasLoadedAll) Loader.OnLoadEnd();

            UpdatePlaylists(data);
        }, (error) => {
            //error
            Debug.LogWarning("TODO: IMPLEMENT ERROR HANDLING");
        });

        //load twitch videos
        TwitchClient.GetTrendingClips("Rocket%20League", 3, (streams) => {
            //success
            _hasLoaded["GetTrendingClips"] = true;
            if (HasLoadedAll && onComplete != null) onComplete.Invoke();
            else if (HasLoadedAll) Loader.OnLoadEnd();

            UpdateStreams(streams);
        }, (error) => {
			//error
			_hasLoaded["GetTrendingClips"] = true;
			Debug.LogWarning("TODO: IMPLEMENT ERROR HANDLING");
        });
	}

	public void SetMainPlayer(PlayerReferenceData playerReference) {
        _mainAccount = playerReference;
	}

	private void UpdatePlaylists(Playlist[] playlists) {
		_playlistView.Set(playlists);
	}

    private void UpdateStreams(Stream[] streams) {
		_twitchView.Set(streams);
	}

    public void SetPlayer(Player player) {
		var database = FindObjectOfType<PlayerDatabase>();
		database.StoreTempPlayer(_mainAccount, player);
		_playerQuickView.Set(player);
    }

	private bool HasLoadedAll {
		get {
			foreach(var kvp in _hasLoaded) {
				if (kvp.Value == false) return false;
			}

			return true;
		}
	}
}
