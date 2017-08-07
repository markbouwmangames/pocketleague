using RLSApi.Net.Models;
using RLSApi.Data;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTool : MonoBehaviour {
    private static Dictionary<int, PlatformData> platformDatas = new Dictionary<int, PlatformData>();

    public static PlatformData GetPlatformData(RlsPlatform platform) {
        return GetPlatformData((int)platform);
    }

    public static PlatformData GetPlatformData(string platformName) {
        var str = platformName.ToLower();

        if (str == "steam") {
            return GetPlatformData(1);
        }

        if (str == "ps4") {
            return GetPlatformData(2);
        }

        if (str == "xbox") {
            return GetPlatformData(3);
        }

        Debug.LogWarning("Platform not defined!");
        return null;
    }

    public static PlatformData GetPlatformData(int platformId) {
        PlatformData platformData;
        if (platformDatas.TryGetValue(platformId, out platformData)) {
            return platformData;
        }

        var resourceName = "";

		if(platformId == 0) {
			resourceName = "Any";
		} else if (platformId == 1) {
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

    public static PlatformData GetPlatformData(Platform platform) {        
        return GetPlatformData(platform.Id);
    }
}
