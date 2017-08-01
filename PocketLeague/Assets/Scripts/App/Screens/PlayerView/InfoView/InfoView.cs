using UnityEngine;
using UnityEngine.UI;
using RLSApi.Net.Models;
using System.Collections;
using UnityEngine.Networking;

public class InfoView : PlayerViewChild {
	[SerializeField]
	private Text _playerName;
	[SerializeField]
	private Text _lastUpdatedAtTime;
	[SerializeField]
	private Image _platformIcon;
	[SerializeField]
	private RawImage _avatarIcon;
    [SerializeField]
    private Sprite _defaultAvatar;

	public override void Set(Player player) {
		_playerName.text = player.DisplayName;

        string date, time;
        CopyDictionary.FormatTime(player.UpdatedAt, out date, out time);
        _lastUpdatedAtTime.text = CopyDictionary.Get("LASTUPDATE", date, time);

        var platform = PlatformTool.GetPlatform(player.Platform);
		_platformIcon.sprite = platform.Icon;

        if (string.IsNullOrEmpty(player.Avatar)) {
            _avatarIcon.texture = _defaultAvatar.texture;
        } else {
            _avatarIcon.texture = _defaultAvatar.texture;
            StartCoroutine(LoadImageRoutine(player.Avatar));
        }
    }

    private IEnumerator LoadImageRoutine(string url) {
        UnityWebRequest www = UnityWebRequest.GetTexture(url);
        yield return www.Send();
        var texture = DownloadHandlerTexture.GetContent(www);
        _avatarIcon.texture = texture;
    }
}
