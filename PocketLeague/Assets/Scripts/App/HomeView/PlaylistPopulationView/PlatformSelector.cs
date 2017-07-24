using System.Collections.Generic;
using UnityEngine;
using RLSApi.Data;
using UnityEngine.UI;

public class PlatformSelector : MonoBehaviour {
    [SerializeField]
    private Toggle _toggleTemplate;
    [SerializeField]
    private PlaylistPopulationView _playlistView;

    private Dictionary<RlsPlatform, bool> platformStatus = new Dictionary<RlsPlatform, bool>();

    void Awake() {
        _toggleTemplate.gameObject.SetActive(false);
    }

    public void Set(RlsPlatform[] rlsPlatforms) {
        for (int i = 0; i < rlsPlatforms.Length; i++) {
            var platform = rlsPlatforms[i];
            var toggle = UITool.CreateField<Toggle>(_toggleTemplate);

            var iconTransform = toggle.transform.FindChild("Icon");
            var image = iconTransform.GetComponent<Image>();
            image.sprite = PlatformTool.GetPlatform(platform).Icon;

            toggle.onValueChanged.AddListener((value) => {
                OnToggleChanged(platform, value);
            });

            platformStatus.Add(platform, toggle.isOn);
        }

        _playlistView.UpdateView(platformStatus);
    }

    private void OnToggleChanged(RlsPlatform platform, bool value) {
        platformStatus[platform] = value;
        _playlistView.UpdateView(platformStatus);
    }
}
