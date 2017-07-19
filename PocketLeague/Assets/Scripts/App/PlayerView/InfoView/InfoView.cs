using UnityEngine;
using UnityEngine.UI;
using RLSApi.Net.Models;

public class InfoView : PlayerViewChild {
	[SerializeField]
	private Text _playerName;
	[SerializeField]
	private Text _lastUpdatedAtTime;
	[SerializeField]
	private Image _platformIcon;
	[SerializeField]
	private Image _avatarIcon;

	public override void Set(Player player) {
		_playerName.text = player.DisplayName;

		string updateTimeS = player.UpdatedAt.ToString("s");
		string updateTimeD = player.UpdatedAt.ToString("d");

		var timeStrings = updateTimeS.Split('T');
		string date = updateTimeD.Replace('/', '-');
		string time = timeStrings[timeStrings.Length-1];
		
		CopyDictionary.Get("LASTUPDATE");

		_lastUpdatedAtTime.text = "Last update: " + date + ", at " + time + " GMT";
	}
}
