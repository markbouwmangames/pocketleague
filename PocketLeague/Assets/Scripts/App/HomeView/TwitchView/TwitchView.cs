using System.Collections.Generic;
using Twitch.Net.Models;
using UnityEngine;
using UnityEngine.UI;

public class TwitchView : MonoBehaviour {
    [SerializeField]
    private TwitchThumbnail _thumbnailTemplate;

    private List<TwitchThumbnail> _thumbnails = new List<TwitchThumbnail>();

    void Awake() {
        _thumbnailTemplate.gameObject.SetActive(false);

		for(var i = 0; i < 3; i++) {
			var thumbnail = UITool.CreateField<TwitchThumbnail>(_thumbnailTemplate);
			_thumbnails.Add(thumbnail);
		}
    }

    public void Set(Stream[] streams) {
		for(var i = 0; i < _thumbnails.Count; i++) {
			var thumbnail = _thumbnails[i];
			thumbnail.gameObject.SetActive(i < streams.Length);
			if (i >= streams.Length) continue;

			var stream = streams[i];
			thumbnail.Set(stream);
		}

		SetHeight(streams.Length);
	}

    private void SetHeight(int numStreams) {
        var thumbnailHeight = 200;
        var spacing = 40;

        var height = (numStreams * thumbnailHeight) + ((numStreams + 1) * spacing);

        var rect = GetComponent<RectTransform>();
        var sizeDelta = rect.sizeDelta;
        sizeDelta.y = height;
        rect.sizeDelta = sizeDelta;

        var layoutElement = GetComponent<LayoutElement>();
        layoutElement.preferredHeight = height;
    }
}
