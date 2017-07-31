using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using RLSApi.Data;

public class PlayerListView : MonoBehaviour {
	[SerializeField]
	private Button _button;
	[SerializeField]
	private Text _playerName;
	[SerializeField]
	private Image _platformIcon;
	[SerializeField]
	private RawImage _avatarIcon;
	[SerializeField]
	private Sprite _defaultAvatar;
	[SerializeField]
	private SimpleRankDisplay _simpleRankDisplay;

	public void Set(PlayerReferenceData playerReference) {
		_playerName.text = playerReference.DisplayName;

		var platform = PlatformTool.GetPlatform(playerReference.Platform);
		_platformIcon.sprite = platform.Icon;

		var database = FindObjectOfType<PlayerDatabase>();
		var player = database.GetLocalPlayerData(playerReference);

		if (string.IsNullOrEmpty(player.Avatar)) {
			_avatarIcon.texture = _defaultAvatar.texture;
		} else {
			_avatarIcon.texture = _defaultAvatar.texture;
			StartCoroutine(LoadImageRoutine(player.Avatar));
		}

		var rankedSeasons = player.RankedSeasons;

		var seasons = new RlsSeason[rankedSeasons.Count];
		rankedSeasons.Keys.CopyTo(seasons, 0);

		var latestSeason = seasons[seasons.Length - 1];
		_simpleRankDisplay.Set(latestSeason, rankedSeasons[latestSeason]);

		_button.onClick.AddListener(() => {
			var app = FindObjectOfType<App>();
			app.SetPlayerView(playerReference);
		});
	}

	private IEnumerator LoadImageRoutine(string url) {
		UnityWebRequest www = UnityWebRequest.GetTexture(url);
		yield return www.Send();
		var texture = DownloadHandlerTexture.GetContent(www);
		_avatarIcon.texture = texture;
	}
}
