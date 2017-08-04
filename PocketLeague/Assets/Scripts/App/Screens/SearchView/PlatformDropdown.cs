using UnityEngine;
using RLSApi.Data;
using RLSApi.Net.Models;
using System;
using UnityEngine.UI;

public class PlatformDropdown : MonoBehaviour {
	void Awake() {
		var dropdown = GetComponent<Dropdown>();
		var platforms = Enum.GetValues(typeof(RlsPlatform));

		foreach(var platform in platforms) {
			var rlsPlatform = (RlsPlatform)(platform);
			var platformData = PlatformTool.GetPlatform((int)rlsPlatform);

			var option = new Dropdown.OptionData(platformData.NameKey, platformData.Icon);
			dropdown.options.Add(option);
		}

		dropdown.value = 0;
		dropdown.RefreshShownValue();
	}
}
