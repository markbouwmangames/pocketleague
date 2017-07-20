using RLSApi.Net.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RLSApi.Data;

public class RankDisplay : MonoBehaviour {
    [SerializeField]
    private PlaylistRankDisplay _playlistRankDisplayTemplate;

    private List<PlaylistRankDisplay> _playlistRankDisplays = new List<PlaylistRankDisplay>();

    void Awake() {
        _playlistRankDisplayTemplate.gameObject.SetActive(false);

        for(int i = 0; i < 4; i++) {
            var prd = CreateField();
            _playlistRankDisplays.Add(prd);
        }
    }

    public void Set(Dictionary<RlsPlaylistRanked, PlayerRank> seasonData) {
        int index = 0;
        foreach (KeyValuePair<RlsPlaylistRanked, PlayerRank> kvp in seasonData) {
            var playlistRankDisplay = _playlistRankDisplays[index];
            playlistRankDisplay.Set(kvp.Key, kvp.Value);
            index++;
        }
    }

    private PlaylistRankDisplay CreateField() {
        var newGO = GameObject.Instantiate(_playlistRankDisplayTemplate.gameObject);

        newGO.transform.SetParent(_playlistRankDisplayTemplate.transform.parent);
        newGO.transform.localScale = _playlistRankDisplayTemplate.transform.localScale;

        newGO.SetActive(true);
        return newGO.GetComponent<PlaylistRankDisplay>();
    }
}
