using System.Collections.Generic;
using UnityEngine;
using RLSApi.Data;
using UnityEngine.UI;
using System;

public class PlatformSelector : MonoBehaviour {
    [SerializeField]
    private Toggle _toggleTemplate;
    [SerializeField]
    private PlaylistPopulationView _playlistView;

    private Dictionary<RlsPlatform, bool> _platformStatus = new Dictionary<RlsPlatform, bool>();
	private List<GameObject> _toggles = new List<GameObject>();

    void Awake() {
        _toggleTemplate.gameObject.SetActive(false);

		var rlsPlatforms = Enum.GetValues(typeof(RlsPlatform));
		CreateToggles(rlsPlatforms);
	}

	public void UpdatePlatforms(RlsPlatform[] rlsPlatforms) {
		var currentPlatforms = new RlsPlatform[_platformStatus.Count];
		_platformStatus.Keys.CopyTo(currentPlatforms, 0);

		var platformList = new List<RlsPlatform>(currentPlatforms);

		for (var i = 0; i < rlsPlatforms.Length; i++) {
			var platform = rlsPlatforms[i];
			if(platformList.Contains(platform)) {
				platformList.Remove(platform);
			} else {
				UpdateToggles(rlsPlatforms);
				break;
			}
		}

		if(platformList.Count > 0) {
			UpdateToggles(rlsPlatforms);
		}

		_playlistView.UpdateView(_platformStatus);
	}

	private void UpdateToggles(RlsPlatform[] rlsPlatforms) {
		foreach(var v in _toggles) {
			Destroy(v);
		}

		_platformStatus.Clear();
		_toggles.Clear();

		CreateToggles(rlsPlatforms);
	}

	private void CreateToggles(Array rlsPlatforms) {
		for (var i = 0; i < rlsPlatforms.Length; i++) {
			var platform = (RlsPlatform)(rlsPlatforms.GetValue(i));
			var toggle = UITool.CreateField<Toggle>(_toggleTemplate);

			var iconTransform = toggle.transform.FindChild("Icon");
			var image = iconTransform.GetComponent<Image>();
			image.sprite = PlatformTool.GetPlatform(platform).Icon;

			toggle.onValueChanged.AddListener((value) => {
				OnToggleChanged(platform, value);
			});

			_toggles.Add(toggle.gameObject);
			_platformStatus.Add(platform, toggle.isOn);
		}
	}

	private void OnToggleChanged(RlsPlatform platform, bool value) {
        _platformStatus[platform] = value;
        _playlistView.UpdateView(_platformStatus);
    }
}
