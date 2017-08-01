using UnityEngine;
using RLSApi.Data;
using System.Collections.Generic;
using RLSApi.Net.Models;
using UnityEngine.UI;

public class SimpleRankDisplay : MonoBehaviour {
	[SerializeField]
	private RectTransform _simplePlaylistRankTemplate;
	
	void Awake() {
		_simplePlaylistRankTemplate.gameObject.SetActive(false);
	}

	public void Set(RlsSeason season, Dictionary<RlsPlaylistRanked, PlayerRank> seasonData) {
		var path = "Data/Seasons/Season" + ((int)(season));
		var selectedSeason = Resources.Load<SeasonData>(path);

		foreach (KeyValuePair<RlsPlaylistRanked, PlayerRank> kvp in seasonData) {
			var simplePlaylistRank = UITool.CreateField<RectTransform>(_simplePlaylistRankTemplate.gameObject);

			var child = simplePlaylistRank.Find("Icon");
			var image = child.GetComponent<Image>();

			var sprite = selectedSeason.Ranks[0].Icon;
			var tier = kvp.Value.Tier;

			if (tier != null) {
				var index = tier.Value;
				sprite = selectedSeason.Ranks[index].Icon;
			}
			image.sprite = sprite;
		}
	}
}
