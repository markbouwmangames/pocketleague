using UnityEngine;
using System;
using System.Collections;
using RLSApi.Net.Models;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;

namespace RLSApi {
    public class RLSApiRequester : MonoBehaviour {
        private struct WWWRequestData {
            public UnityWebRequest WWW;
            public Action<string> OnSuccess;
            public Action<Error> OnFail;
        }

        private string _url;
        private string _authKey;
        private bool _debug;
        private bool _throttleRequests;

        private Queue<WWWRequestData> _requestQueue = new Queue<WWWRequestData>();
        private int _rateLimitRemaining;
        private DateTime _rateLimitResetRemaining;

        public void Init(string url, string authKey, bool debug, bool throttleRequests = true) {
            //set initial values passed by RLSClient
            _url = url;
            _authKey = authKey;
            _debug = debug;
            _throttleRequests = throttleRequests;

            //set standard rate limiter
            _rateLimitRemaining = 2;
            _rateLimitResetRemaining = DateTime.UtcNow.AddSeconds(1);

            if (_throttleRequests) {
                StartCoroutine(ThrottleRequests());
            }
        }

        public void Get(string urlPosfix, Action<string> onSuccess, Action<Error> onFail) {
            var www = GetRequestWWW(urlPosfix);

            if (_throttleRequests) _requestQueue.Enqueue(new WWWRequestData() {
                WWW = www,
                OnSuccess = onSuccess,
                OnFail = onFail
            });
            else DoWWW(www, onSuccess, onFail);
        }

        public void Post(string urlPosfix, string postData, Action<string> onSuccess, Action<Error> onFail) {
            var www = GetPostWWW(urlPosfix, postData);

            if (_throttleRequests) _requestQueue.Enqueue(new WWWRequestData() {
                WWW = www,
                OnSuccess = onSuccess,
                OnFail = onFail
            });
            else DoWWW(www, onSuccess, onFail);
        }

        private void DoWWW(UnityWebRequest www, Action<string> onSuccess, Action<Error> onFail) {
            //start request routine
            StartCoroutine(WebRoutine(www, (data) => {
                //on succes callback
                onSuccess.Invoke(data);
            }, (data) => {
                //on fail callback
                onFail.Invoke(data);
            }));
        }

        private UnityWebRequest GetRequestWWW(string postfix) {
            var url = _url + postfix;
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.SetRequestHeader("Authorization", _authKey);
            return www;
        }

        private UnityWebRequest GetPostWWW(string postfix, string postData) {
            var url = _url + postfix;
            var www = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(postData);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", _authKey);
            return www;
        }

        private IEnumerator ThrottleRequests() {
            while (true) {
                yield return null;
                //wait until we're allowed to send requests again
                if (_rateLimitRemaining == 0) {
                    var startTime = DateTime.UtcNow;
                    var difference = _rateLimitResetRemaining - startTime;

                    if (difference > TimeSpan.Zero) {
                        yield return new WaitForSeconds((float)difference.TotalSeconds);
                    }
                }

                //send request
                if (_requestQueue.Count > 0) {
                    var request = _requestQueue.Dequeue();
                    yield return StartCoroutine(WebRoutine(request.WWW, request.OnSuccess, (data) => {
                        //server too busy, retry
                        if (data.StatusCode == 429) _requestQueue.Enqueue(request);
                        else request.OnFail(data);
                    }));
                }
            }
        }

        private IEnumerator WebRoutine(UnityWebRequest www, Action<string> onSuccess, Action<Error> onFail) {
            //wait for response
            if (_debug) Debug.Log("GET DATA from " + www.url);
            yield return www.Send();

            if (!www.isError && www.responseCode >= 200 && www.responseCode <= 299) {
                //server returned data
                if (_debug) Debug.Log("GOT DATA from " + www.url + ", data: " + www.downloadHandler.text);
                onSuccess.Invoke(www.downloadHandler.text);
            } else {
                Error error = new Error();

                if (www.isError) {
                    //www request error
                    if (_debug) Debug.LogError("GOT WWW REQUEST ERROR from " + www.url + ", error: " + www.error);
                    error.StatusCode = www.responseCode;
                    error.Message = www.error;
                } else {
                    //server returned error
                    if (_debug) Debug.LogError("GOT SERVER ERROR from " + www.url + ", error: " + www.downloadHandler.text);
                    error = JsonConvert.DeserializeObject<Error>(www.downloadHandler.text);
                }

                onFail.Invoke(error);
            }

            //check headers for rate limit
            SetRateLimit(www);
        }

        private void SetRateLimit(UnityWebRequest www) {
            if (www.isError) return;

            //get headers
            var xRateLimitRemainingHeader = www.GetResponseHeader("x-rate-limit-remaining");
            var xRateLimitResetRemainingHeader = www.GetResponseHeader("x-rate-limit-reset-remaining");

            //parse headers to rate limits
            int rateLimitRemaining, rateLimitResetRemaining;
            if (int.TryParse(xRateLimitRemainingHeader, out rateLimitRemaining)
                && int.TryParse(xRateLimitResetRemainingHeader, out rateLimitResetRemaining)) {

                //set rate limit and time till renewal
                _rateLimitRemaining = rateLimitRemaining;
                _rateLimitResetRemaining = DateTime.UtcNow.AddMilliseconds(rateLimitResetRemaining);
            }
        }
    }
}