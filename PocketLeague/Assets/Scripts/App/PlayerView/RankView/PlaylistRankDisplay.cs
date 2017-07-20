using RLSApi.Net.Models;
using UnityEngine;
using UnityEngine.UI;
using RLSApi.Data;

public class PlaylistRankDisplay : MonoBehaviour {
    [SerializeField]
    private Text title;
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text rank;
    [SerializeField]
    private Text division;
    [SerializeField]
    private Text rating;
    
    public void Set(RlsPlaylistRanked rlsPlaylistRanked, PlayerRank playerRank) {
        title.text = rlsPlaylistRanked.ToString();

        if (playerRank.Division != null) {
            var div = playerRank.Division.Value;
            division.text = "Division " + ToRoman(div);
        }

        if (playerRank.Tier != null) {
            rank.text = playerRank.Tier.Value.ToString();
        }

        rating.text = playerRank.RankPoints.ToString();
    }

    private string ToRoman(int number) {
        Debug.Log(number);

        if (number == 1) {
            return "I";
        }
        if (number == 2) {
            return "II";
        }
        if (number == 3) {
            return "III";
        }
        if (number == 4) {
            return "IV";
        }
        if (number == 5) {
            return "V";
        }

        return "";
    }
}
