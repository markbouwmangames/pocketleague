using System.Collections.Generic;
using Twitch.Net.Models;
using UnityEngine;
using UnityEngine.UI;

public class TwitchView : MonoBehaviour {
    [SerializeField]
    private TwitchThumbnail _thumbnailTemplate;

    private List<TwitchThumbnail> _thumbnails = new List<TwitchThumbnail>();
    private List<string> _currentStreams = new List<string>();

    void Awake() {
        _thumbnailTemplate.gameObject.SetActive(false);
    }

    public void Set(Stream[] streams) {
        CreateThumbnails(streams);
    }

    private void CreateThumbnails(Stream[] streams) {
        bool shouldUpdate = false;
        foreach (var stream in streams) {
            if (_currentStreams.Contains(stream.Channel.URL) == false) {
                shouldUpdate = true;
                break;
            }
        }

        if (!shouldUpdate) return;

        if (_thumbnails != null) {
            foreach (var thumbnail in _thumbnails) {
                Destroy(thumbnail.gameObject);
            }
            _thumbnails.Clear();
        }

        foreach (var stream in streams) {
            var thumbnail = UITool.CreateField<TwitchThumbnail>(_thumbnailTemplate);
            thumbnail.Set(stream);
            _thumbnails.Add(thumbnail);
            _currentStreams.Add(stream.Channel.URL);
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
