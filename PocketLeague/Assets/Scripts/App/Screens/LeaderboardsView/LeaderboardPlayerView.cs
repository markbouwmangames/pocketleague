 using UnityEngine;
using UnityEngine.UI;
using RLSApi.Net.Models;
using System.Collections;
using UnityEngine.Networking;
using RLSApi.Data;

public class LeaderboardPlayerView : MonoBehaviour {
	[SerializeField]
	private Text _rank;
	[SerializeField]
	private Text _username;
	[SerializeField]
	private Text _stat;
	[SerializeField]
	private Image _platformIcon;
	[SerializeField]
	private RawImage _avatarIcon;
	[SerializeField]
	private Sprite _defaultAvatar;

	private PlayerReferenceData _playerReferenceData;

	public void Set(int rank, int stat, Player player) {

		_rank.text = rank.ToString();
		_stat.text = stat.ToString();
		_username.text = player.DisplayName;

		var platform = PlatformTool.GetPlatformData(player.Platform);
		_platformIcon.sprite = platform.Icon;

		PlayerTool.LoadAvatar(_avatarIcon, _defaultAvatar, player);

		_playerReferenceData = player.Convert();
		var button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);
	}

	private void OnClick() {
		var app = FindObjectOfType<App>();
		app.SetPlayerView(_playerReferenceData);
	}
}
