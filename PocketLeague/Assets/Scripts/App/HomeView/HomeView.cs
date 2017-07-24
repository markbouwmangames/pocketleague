using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RLSApi.Net.Models;
using RLSApi;

public class HomeView : MonoBehaviour {
    [SerializeField]
    private RankView _rankView;

    [SerializeField]
    private PlaylistPopulationView _playlistView;

    void Awake() {
        RLSClient.GetPlaylists(UpdatePlaylists, null);

        RLSClient.GetPlayer(RLSApi.Data.RlsPlatform.Ps4, "Mefoz", (player) => {
            SetPlayer(player);
        }, null);
    }

    private void UpdatePlaylists(Playlist[] playlists) {
        _playlistView.Set(playlists);
    }

    public void SetPlayer(Player player) {
        _rankView.Set(player);
    }
}
