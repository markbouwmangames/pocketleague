using UnityEngine;
using RLSApi.Net.Models;
using RLSApi;
using Twitch;
using Twitch.Net.Models;
using System.Collections.Generic;

public class HomeView : BaseView {
    [SerializeField]
    private RankView _rankView;

    [SerializeField]
    private PlaylistPopulationView _playlistView;

    [SerializeField]
    private TwitchView _twitchView;
	
	private Dictionary<string, bool> _hasLoaded = new Dictionary<string, bool>();

	protected override void Init() {
		_hasLoaded.Add("GetPlaylists", false);
		_hasLoaded.Add("GetPlayer", false);
		_hasLoaded.Add("GetTrendingClips", false);

		base.Init();
	}

	protected override void UpdateView() {
		Loader.OnLoadStart();

		_hasLoaded["GetPlaylists"] = false;
		_hasLoaded["GetPlayer"] = false;
		_hasLoaded["GetTrendingClips"] = false;

		RLSClient.GetPlaylists(UpdatePlaylists, null);

		TwitchClient.GetTrendingClips("Rocket%20League", 3, (streams) => {
			UpdateStreams(streams);
		}, null);
	}

	public void SetPlayer(PlayerReferenceData playerReference) {
		RLSClient.GetPlayer(playerReference.Platform, playerReference.DisplayName, (player) => {
			//succes
			SetPlayer(player);
		}, (error) => {
			//error
			Debug.LogWarning("TODO: IMPLEMENT ERROR HANDLING");
		});
	}

	private void UpdatePlaylists(Playlist[] playlists) {
		_hasLoaded["GetPlaylists"] = true;
		_playlistView.Set(playlists);
		if (HasLoadedAll) Loader.OnLoadEnd();
	}

    private void UpdateStreams(Stream[] streams) {
		_hasLoaded["GetTrendingClips"] = true;
		_twitchView.Set(streams);
		if (HasLoadedAll) Loader.OnLoadEnd();
	}

    public void SetPlayer(Player player) {
		_hasLoaded["GetPlayer"] = true;
		_rankView.Set(player);
		if (HasLoadedAll) Loader.OnLoadEnd();
    }

	private bool HasLoadedAll {
		get {
			foreach(var kvp in _hasLoaded) {
				Debug.Log(kvp.Key + ", " + kvp.Value);
				if (kvp.Value == false) return false;
			}

			return true;
		}
	}
}
