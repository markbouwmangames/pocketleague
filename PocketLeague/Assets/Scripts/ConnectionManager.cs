using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour {
    private static ConnectionManager _instance;

    private static ConnectionManager instance {
        get {
            if (_instance == null) _instance = new GameObject("ConnectionManager").AddComponent<ConnectionManager>();
            return _instance;
        }
    }


    public static void Get() {

    }

    public static void Post() {

    }

}
