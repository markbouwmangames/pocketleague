using Newtonsoft.Json;
using System;
using Twitch.Net;
using Twitch.Net.Models;
using UnityEngine;
namespace Twitch {
    public class TwitchClient {
        /// <summary>
        /// Initial values used for the API
        /// </summary>
        private static readonly string clientId = "fsbs0ur09zmbi3drjh9f4b8snelc17";
        private static readonly bool debug = true;

        public static void GetTrendingClips(string gameTitle, int limit, Action<Stream[]> onSuccess, Action<string> onFail) {
            api.GetTrendingClips(gameTitle, limit, (data) => {
                var streamData = JsonConvert.DeserializeObject<StreamData>(data);
                if(onSuccess != null) onSuccess.Invoke(streamData.Streams);
            }, onFail);
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
                    _twitchApiRequester.Init(clientId, debug);
                }

                return _twitchApiRequester;
            }
        }
        #endregion
    }
}