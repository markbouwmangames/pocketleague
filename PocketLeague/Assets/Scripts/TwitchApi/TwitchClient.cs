using Newtonsoft.Json;
using System;
using System.Runtime.InteropServices;
using Twitch.Net;
using Twitch.Net.Models;
using UnityEngine;
namespace Twitch {
    public class TwitchClient {
        /// <summary>
        /// Initial values used for the API
        /// </summary>
        private static readonly string clientId = "fsbs0ur09zmbi3drjh9f4b8snelc17";
        private static readonly bool debug = false;
        
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern bool hasTwitch();
#elif UNITY_ANDROID && !UNITY_EDITOR
        private static bool hasTwitch() {
            string bundleId = "tv.twitch.android";

            AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
            AndroidJavaObject launchIntent = null;
            try {
                launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
            } catch (Exception ex) {
                Debug.Log("exception" + ex.Message);
            }
            if (launchIntent == null)
                return false;
            return true;
        }
#else 
        private static bool hasTwitch() {
            return false;
        }
#endif

        public static void GetTrendingClips(string gameTitle, int limit, Action<Stream[]> onSuccess, Action<string> onFail) {
            api.GetTrendingClips(gameTitle, limit, (data) => {
                var streamData = JsonConvert.DeserializeObject<StreamData>(data);
                if(onSuccess != null) onSuccess.Invoke(streamData.Streams);
            }, onFail);
        }

        public static void OpenURL(string streamName) {
            if(hasTwitch()) {
                var url = "twitch://stream/" + streamName;
                Application.OpenURL(url);
            } else {
                var url = "https://www.twitch.tv/" + streamName;
                Application.OpenURL(url);
            }
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