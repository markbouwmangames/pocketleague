using UnityEngine;
using RLSApi.Net.Models;
using RLSApi;
using Twitch;
using Twitch.Net.Models;

public class HomeView : MonoBehaviour {
    [SerializeField]
    private RankView _rankView;

    [SerializeField]
    private PlaylistPopulationView _playlistView;

    [SerializeField]
    private TwitchView _twitchView;

    void Awake() {
        RLSClient.GetPlaylists(UpdatePlaylists, null);

        RLSClient.GetPlayer(RLSApi.Data.RlsPlatform.Ps4, "Mefoz", (player) => {
            SetPlayer(player);
        }, null);

        TwitchClient.GetTrendingClips("Rocket%20League", 3, (streams) => {
            UpdateStreams(streams);
        }, null);
    }

    private void UpdatePlaylists(Playlist[] playlists) {
        _playlistView.Set(playlists);
    }

    private void UpdateStreams(Stream[] streams) {
        _twitchView.Set(streams);
    }

    public void SetPlayer(Player player) {
        _rankView.Set(player);
    }
}
