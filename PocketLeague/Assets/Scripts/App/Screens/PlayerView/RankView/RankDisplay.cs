﻿using RLSApi.Net.Models;
using System.Collections.Generic;
using UnityEngine;
using RLSApi.Data;

public class RankDisplay : MonoBehaviour {
    [SerializeField]
    private PlaylistRankDisplay _playlistRankDisplayTemplate;

    private List<PlaylistRankDisplay> _playlistRankDisplays = new List<PlaylistRankDisplay>();

	private SeasonData _selectedSeason;

    void Awake() {
        _playlistRankDisplayTemplate.gameObject.SetActive(false);

        for(int i = 0; i < 4; i++) {
            var prd = UITool.CreateField<PlaylistRankDisplay>(_playlistRankDisplayTemplate.gameObject);
            _playlistRankDisplays.Add(prd);
        }
    }

    public void Set(RlsSeason season, Dictionary<RlsPlaylistRanked, PlayerRank> seasonData) {
		var path = "Data/Seasons/Season" + ((int)(season));
		_selectedSeason = Resources.Load<SeasonData>(path);
		
		int index = 0;
        foreach (KeyValuePair<RlsPlaylistRanked, PlayerRank> kvp in seasonData) {
            var playlistRankDisplay = _playlistRankDisplays[index];
            playlistRankDisplay.Set(_selectedSeason, kvp.Key, kvp.Value);
            index++;
        }
    }
}