using System.Collections.Generic;
using UnityEngine;
using RLSApi.Net.Models;
using RLSApi.Data;
using UnityEngine.UI;

public class PlaylistPopulationView : MonoBehaviour {
    [SerializeField]
    private PlatformSelector _platformSelector;

	[SerializeField]
	private PlaylistPopulationDisplay _playlistPopulationDisplayTemplate;

	[SerializeField]
	private List<RlsPlaylist> _hiddenPlaylists;

	private PlaylistPopulationDisplay[] _displays;

    private Playlist[] _playlists;
    private List<RlsPlatform> _platforms;
    private List<RlsPlaylist> _rlsPlaylistIds;

    void Awake() {
        _playlistPopulationDisplayTemplate.gameObject.SetActive(false);
    }

    public void Set(Playlist[] playlists) {
        _playlists = playlists;

        _platforms = new List<RlsPlatform>();
        _rlsPlaylistIds = new List<RlsPlaylist>();

        foreach (var playlist in playlists) {
            if (_platforms.Contains(playlist.Platform) == false) {
                _platforms.Add(playlist.Platform);
            }

            if (_rlsPlaylistIds.Contains(playlist.Id) == false) {
                _rlsPlaylistIds.Add(playlist.Id);
            }
        }

        SetupDisplays(_rlsPlaylistIds);
        SetHeight(_rlsPlaylistIds.Count);
		_platformSelector.UpdatePlatforms(_platforms.ToArray());
    }

    private void SetupDisplays(List<RlsPlaylist> rlsPlaylistIds) {
        if (_displays != null) return;
		
        var num = rlsPlaylistIds.Count - _hiddenPlaylists.Count;
        _displays = new PlaylistPopulationDisplay[num];

        for (var i = 0; i < num; i++) {
            var ppd = UITool.CreateField<PlaylistPopulationDisplay>(_playlistPopulationDisplayTemplate);
            _displays[i] = ppd;
        }
    }

    private void SetHeight(int numPlaylists) {
        var prefPlatformSelectorHeight = _platformSelector.GetComponent<LayoutElement>().preferredHeight;
        var prefPlaylistPopulationDisplayHeight = _playlistPopulationDisplayTemplate.GetComponent<LayoutElement>().preferredHeight;
        var height = prefPlatformSelectorHeight + (prefPlaylistPopulationDisplayHeight * numPlaylists);
        GetComponent<LayoutElement>().preferredHeight = height;

        var rect = GetComponent<RectTransform>();
        var size = rect.sizeDelta;
        size.y = height;
        rect.sizeDelta = size;
    }

    public void UpdateView(Dictionary<RlsPlatform, bool> platformStatus) {
        var selectedPlatforms = new List<RlsPlatform>();
        foreach(var kvp in platformStatus) {
            if (kvp.Value == true) {
                selectedPlatforms.Add(kvp.Key);
            }
        }

        var totalPopulation = 0;
        for (var i = 0; i < _rlsPlaylistIds.Count; i++) {
            var playlist = _rlsPlaylistIds[i];
            totalPopulation += GetPopulation(playlist, selectedPlatforms.ToArray());
        }

		int index = 0;
        for (var i = 0; i < _rlsPlaylistIds.Count; i++) {
            var playlist = _rlsPlaylistIds[i];
            var population = GetPopulation(playlist, selectedPlatforms.ToArray());

			if (_hiddenPlaylists.Contains(playlist)) continue;
            _displays[index++].Set(playlist, population, totalPopulation);
        }
    }

    private int GetPopulation(RlsPlaylist rlsPlaylistId, params RlsPlatform[] platforms) {
        var population = 0;

        foreach (var playlist in _playlists) {
            if (playlist.Id != rlsPlaylistId) continue;

            bool selectedPlatform = false;
            foreach (var platform in platforms) {
                if (platform == playlist.Platform) selectedPlatform = true;
            }
            if (!selectedPlatform) continue;

            population += playlist.Population.Players;
        }

        return population;
    }
}
