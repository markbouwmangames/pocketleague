using RLSApi.Data;
using UnityEngine;

//https://rltracker.pro/tier_breakdown
//https://rocketleague.tracker.network/distribution/10

[System.Serializable]
public class DivisionData {
    public PlaylistDivisionData[] PlaylistDivisionRatings;
}

[System.Serializable]
public class PlaylistDivisionData {
    public RlsPlaylistRanked Playlist;
    public int minRating;
    public int maxRating;
}