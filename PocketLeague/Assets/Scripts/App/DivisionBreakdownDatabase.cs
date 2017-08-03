using UnityEngine;
using System.Collections.Generic;
using System;
using RLSApi;
using RLSApi.Data;
using Newtonsoft.Json;
using RLSApi.Util;

public class DivisionBreakdownDatabase : MonoBehaviour {
	private const string _lastUpdateLocation = "::breakdownDatabase::lastUpdateLocation";
	private const string _dataLocation = "::breakdownDatabase::dataLocation";

	public Dictionary<RlsPlaylistRanked, List<DivisionData[]>> _rankData = new Dictionary<RlsPlaylistRanked, List<DivisionData[]>>();

	void Awake() {
		var data = PlayerPrefs.GetString(_dataLocation, "");
		if(string.IsNullOrEmpty(data)) {
			LoadDataFromApi();
		} else {
			var timeData = PlayerPrefs.GetString(_lastUpdateLocation);
			var time = JsonConvert.DeserializeObject<DateTimeOffset>(timeData);

			var difference = TimeUtil.DifferenceDays(DateTimeOffset.UtcNow, time);
			if (difference > 2) {
				LoadDataFromApi();
			} else {
				ConvertData(data);
			}
		}
	}

	private void RequestData(RlsPlaylistRanked playlist, Action onComplete) {
		var list = new List<DivisionData[]>();

		RLSClient.GetDivisionBreakdown(playlist, (data) => {
			foreach (var divisionList in data) {
				var collection = new DivisionData[divisionList.Count];

				for (var i = 0; i < divisionList.Count; i++) {
					var divisionBreakdown = divisionList[i];
					var div = new DivisionData() {
						MinRating = divisionBreakdown[0],
						MaxRating = divisionBreakdown[1]
					};
					collection[i] = div;
				}
				list.Add(collection);
			}

			_rankData.Add(playlist, list);
			if (onComplete != null) onComplete.Invoke();
		}, (error) => {
			//TODO: Error handling
		});
	}

	private void LoadDataFromApi() {
		var playlists = Enum.GetValues(typeof(RlsPlaylistRanked));
		for (var i = 0; i < playlists.Length; i++) {
			var playlist = (RlsPlaylistRanked)(playlists.GetValue(i));
			RequestData(playlist, () => {
				if (_rankData.Count == playlists.Length) {
					StoreData();
				}
			});
		}
	}

	private void ConvertData(string data) {
		_rankData = JsonConvert.DeserializeObject<Dictionary<RlsPlaylistRanked, List<DivisionData[]>>>(data);
	}

	private void StoreData() {
		var data = JsonConvert.SerializeObject(_rankData);
		PlayerPrefs.SetString(_dataLocation, data);

		var time = DateTimeOffset.UtcNow;
		var timeData = JsonConvert.SerializeObject(time);
		PlayerPrefs.SetString(_lastUpdateLocation, timeData);
	}

	public bool HasDivisionData() {
		var playlists = Enum.GetValues(typeof(RlsPlaylistRanked));
		return _rankData.Count == playlists.Length;
	}

	public DivisionData GetDivisionData(RlsPlaylistRanked playlist, int tier, int division) {
		return _rankData[playlist][division][tier];
	}
}

public class DivisionData {
	public int MinRating;
	public int MaxRating;
}
