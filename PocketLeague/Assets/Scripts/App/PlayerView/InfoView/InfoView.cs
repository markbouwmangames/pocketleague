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
        CopyDictionary.SetLanguage(Language.EN);
        _lastUpdatedAtTime.text = CopyDictionary.Get("LASTUPDATE", date, time);

		var resourceName = "";
		var id = int.Parse(player.Platform.Id);

		if(id == 1) {
			resourceName = "Steam";
		} else if(id == 2) {
			resourceName = "PS4";
		} else if(id == 3) {
			resourceName = "XBOX";
		}

		var platform = Resources.Load<PlatformData>("Data/Platforms/" + resourceName);
		_platformIcon.sprite = platform.Icon;
    }
}
