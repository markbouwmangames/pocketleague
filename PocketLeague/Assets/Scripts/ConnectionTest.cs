using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionTest : MonoBehaviour {
    private readonly string url = "https://api.rocketleaguestats.com/v1/";
    private readonly string authKey = "ZXT5OU2S4B53F0GQ3UZHL7620K1MBV9G";

    IEnumerator Start() {
        var added = "data/tiers";

        //add auth key
        var form = new WWWForm();
        form.headers.Add("Authorization", authKey);

        var headerInfo = new Dictionary<string, string>();
        headerInfo.Add("Authorization", authKey);

        //www request
        var www = new WWW(url + added, null, headerInfo);

        //wait for reply
        yield return www;

        //debug outcome
        if (string.IsNullOrEmpty(www.error) == false) {
            Debug.Log(www.error);
        } else {
            Debug.Log(www.text);
        }
    }
}
