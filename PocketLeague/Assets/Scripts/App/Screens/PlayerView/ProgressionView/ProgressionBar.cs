using UnityEngine;
using UnityEngine.UI;
using RLSApi.Data;
using RLSApi.Net.Models;

public class ProgressionBar : MonoBehaviour {
	[SerializeField]
	private Text _playlistName;
	[SerializeField]
	private Text _currentRating;
	[SerializeField]
	private Text _divisionDownRatings;
	[SerializeField]
	private Text _divisionUpRatings;
	[SerializeField]
	private Text _divisionDown;
	[SerializeField]
	private Text _divisionUp;
	[SerializeField]
	private Image _progression;

	[SerializeField]
	private GameObject _progressionBar;
	[SerializeField]
	private GameObject _unrankedOverlay;

	public void Set(RlsPlaylistRanked playlist, PlayerRank rank, SeasonData currentSeason) {
		_playlistName.text = CopyDictionary.Get(playlist.ToString().ToUpper());
		
		if(rank.Tier != null && rank.Tier.Value != 0) {
			_progressionBar.SetActive(true);
			_unrankedOverlay.SetActive(false);

			var divisionBreakdown = FindObjectOfType<DivisionBreakdownDatabase>();

			var breakdown = divisionBreakdown.GetDivisionData(playlist, rank.Tier.Value - 1, rank.Division.Value);
			
			var min = breakdown.MinRating;
			var max = breakdown.MaxRating;
			var current = rank.RankPoints;

			var toDemote = (current - min);
			var toPromote = (max - current);

			_currentRating.text = current.ToString();
			_divisionDownRatings.text = min.ToString();
			_divisionUpRatings.text = max.ToString();

			_divisionDown.text = "-" + toDemote;
			_divisionUp.text = "+" + toPromote;

			var isHighestRank = rank.Tier == currentSeason.Ranks.Length - 1;
			_divisionUp.gameObject.SetActive(!isHighestRank);

			if (isHighestRank) max = min + 50;

			var percentage = ((float)(current - min)) / ((float)(max - min));
			_progression.fillAmount = percentage;
		} else {
			_progressionBar.SetActive(false);
			_unrankedOverlay.SetActive(true);
		}
	}
}
