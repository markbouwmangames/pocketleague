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
		var player = database.GetTrackedPlayerData(playerReference);
		
		PlayerTool.LoadAvatar(_avatarIcon, _defaultAvatar, player);
		
		_simpleRankDisplay.Set(Constants.LatestSeason, player.CurrentSeason());

		_button.onClick.AddListener(() => {
			var app = FindObjectOfType<App>();
			app.SetPlayerView(playerReference);
		});
	}
}
