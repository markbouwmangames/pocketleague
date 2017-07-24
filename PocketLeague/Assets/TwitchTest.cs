using System.Collections;
using System.Collections.Generic;
using Twitch;
using UnityEngine;

public class TwitchTest : MonoBehaviour {
    void Awake() {
        var title = "Rocket%20League";
        var limit = 2;

        TwitchClient.GetTrendingClips(title, limit, (data) => {
            Debug.Log(data);
        }, null);
    }
}
