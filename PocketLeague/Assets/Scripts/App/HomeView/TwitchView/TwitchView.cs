using System.Collections;
using System.Collections.Generic;
using Twitch;
using UnityEngine;

public class TwitchView : MonoBehaviour {
    void Awake() {
        var title = "Rocket%20League";
        var limit = 2;

        TwitchClient.GetTrendingClips(title, limit, (data) => {
            var str = data[0];
            Debug.Log(str.Preview.Template);
        }, null);
    }
}
