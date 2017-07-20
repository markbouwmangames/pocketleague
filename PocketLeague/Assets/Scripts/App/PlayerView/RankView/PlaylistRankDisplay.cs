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
    private Text[] ranks;
    [SerializeField]
    private Text division;
    [SerializeField]
    private Text rating;

	[SerializeField]
	private GameObject withDivisionContainer;
	[SerializeField]
	private GameObject withoutDivisionContainer;

	private SeasonData _currentSeason;
    
    public void Set(SeasonData seasonData, RlsPlaylistRanked rlsPlaylistRanked, PlayerRank playerRank) {
		_currentSeason = seasonData;
		SetTitle(rlsPlaylistRanked);
		SetRankText(playerRank);
		SetRankIcon(playerRank);
		SetDivisionText(playerRank);
		SetRating(playerRank);
	}

	private void SetTitle(RlsPlaylistRanked rlsPlaylistRanked) {
		var titleKey = rlsPlaylistRanked.ToString().ToUpper();
		title.text = CopyDictionary.Get(titleKey);
	}

	private void SetRankText(PlayerRank playerRank) {
		var key = _currentSeason.Ranks[0].NameKey;
		if (playerRank.Tier != null) {
			var index = playerRank.Tier.Value;
			key = _currentSeason.Ranks[index].NameKey;
		}

		var text = CopyDictionary.Get(key);
		foreach (var rank in ranks) {
			rank.text = text;
		}
	}

	private void SetRankIcon(PlayerRank playerRank) {
		var sprite = _currentSeason.Ranks[0].Icon;
		if (playerRank.Tier != null) {
			var index = playerRank.Tier.Value;
			sprite = _currentSeason.Ranks[index].Icon;
		}
		icon.sprite = sprite;
	}

	private void SetDivisionText(PlayerRank playerRank) {
		var text = "";
		bool hasDivisions = false;

		if (_currentSeason.HasDivisions) {
			hasDivisions = true;
			if (playerRank.Division != null) {
				var index = playerRank.Tier.Value;
				if (_currentSeason.Ranks[index].HasDivisions) {
					var div = playerRank.Division.Value;
					text = CopyDictionary.Get("DIVISION", ToRoman(div).ToString());
				}
			}
		}

		division.text = text;
		withDivisionContainer.SetActive(hasDivisions);
		withoutDivisionContainer.SetActive(!hasDivisions);
	}

	private void SetRating(PlayerRank playerRank) {
		rating.text = playerRank.RankPoints.ToString();
	}

	private string ToRoman(int number) {
        if (number == 0) {
            return "I";
        }
        if (number == 1) {
            return "II";
        }
        if (number == 2) {
            return "III";
        }
        if (number == 3) {
            return "IV";
        }
        if (number == 4) {
            return "V";
        }

        return "N/A";
    }
}
