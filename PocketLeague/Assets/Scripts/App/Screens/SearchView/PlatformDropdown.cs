using UnityEngine;
using RLSApi.Data;
using RLSApi.Net.Models;
using System;
using UnityEngine.UI;

public class PlatformDropdown : MonoBehaviour {
	private RlsPlatform[] values;

	void Awake() {
		var dropdown = GetComponent<Dropdown>();
		var platforms = Enum.GetValues(typeof(RlsPlatform));
		values = new RlsPlatform[platforms.Length];

		for(var i = 0; i < platforms.Length; i++) { 
			var rlsPlatform = (RlsPlatform)(platforms.GetValue(i));
			values[i] = rlsPlatform;

			var platformData = PlatformTool.GetPlatformData((int)rlsPlatform);

			var option = new Dropdown.OptionData(platformData.NameKey, platformData.Icon);
			dropdown.options.Add(option);
		}

		dropdown.value = 0;
		dropdown.RefreshShownValue();
	}

	public RlsPlatform GetValue() {
		var dropdown = GetComponent<Dropdown>();
		var index = dropdown.value;
		return values[index];
	}
}
