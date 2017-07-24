using System;
using Twitch.Net;
using UnityEngine;
namespace Twitch {
    public class TwitchClient {
        public static void GetTrendingClips(string gameTitle, int limit, Action<string> onSuccess, Action<string> onFail) {
            api.GetTrendingClips(gameTitle, limit, onSuccess, onFail);
        }

        #region INTERNAL
        private static TwitchApiRequester _twitchApiRequester;
        private static TwitchApiRequester api {
            get {
                if (_twitchApiRequester == null) {
                    //create gameObject
                    var go = new GameObject("TwitchAPI");
                    GameObject.DontDestroyOnLoad(go);

                    //add api
                    _twitchApiRequester = go.AddComponent<TwitchApiRequester>();
                }

                return _twitchApiRequester;
            }
        }
        #endregion
    }
}