using System.Collections;
using Twitch;
using Twitch.Net.Models;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TwitchThumbnail : MonoBehaviour {
    [SerializeField]
    private RawImage _preview;
    [SerializeField]
    private Text _title;
    [SerializeField]
    private Text _viewers;
    [SerializeField]
    private Text _followers;



    public void Set(Stream stream) {
        _title.text = stream.Channel.DisplayName;
        _viewers.text = stream.Viewers.ToString();
        _followers.text = stream.Channel.Followers.ToString();

        var previewUrl = stream.Preview.Template;
        StartCoroutine(LoadThumbnailRoutine(previewUrl, 300, 200));

        var button = GetComponent<Button>();
        button.onClick.AddListener(() => {
            TwitchClient.OpenURL(stream.Channel.StreamName);
        });
    }

    private IEnumerator LoadThumbnailRoutine(string url, int width, int height) {
        url = url.Replace("{width}", width.ToString()).Replace("{height}", height.ToString());

        UnityWebRequest www = UnityWebRequest.GetTexture(url);
        yield return www.Send();
        var texture = DownloadHandlerTexture.GetContent(www);
        _preview.texture = texture;
    }
}
