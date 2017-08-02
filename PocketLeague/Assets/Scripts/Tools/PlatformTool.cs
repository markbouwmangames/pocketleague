using RLSApi.Net.Models;
using RLSApi.Data;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTool : MonoBehaviour {
    private static Dictionary<int, PlatformData> platformDatas = new Dictionary<int, PlatformData>();

    public static PlatformData GetPlatform(RlsPlatform platform) {
        return GetPlatform((int)platform);
    }

    public static PlatformData GetPlatform(string platformName) {
        var str = platformName.ToLower();

        if (str == "steam") {
            return GetPlatform(1);
        }

        if (str == "ps4") {
            return GetPlatform(2);
        }

        if (str == "xbox") {
            return GetPlatform(3);
        }

        Debug.LogWarning("Platform not defined!");
        return null;
    }

    public static PlatformData GetPlatform(int platformId) {
        PlatformData platformData;
        if (platformDatas.TryGetValue(platformId, out platformData)) {
            return platformData;
        }

        var resourceName = "";

        if (platformId == 1) {
            resourceName = "Steam";
        } else if (platformId == 2) {
            resourceName = "PS4";
        } else if (platformId == 3) {
            resourceName = "XBOX";
        } else {
            Debug.LogWarning("Platform not defined!");
            return null;
        }

        var platform = Resources.Load<PlatformData>("Data/Platforms/" + resourceName);
        platformDatas.Add(platformId, platform);
        return platform;
    }

    public static PlatformData GetPlatform(Platform platform) {        
        return GetPlatform(platform.Id);
    }
}
