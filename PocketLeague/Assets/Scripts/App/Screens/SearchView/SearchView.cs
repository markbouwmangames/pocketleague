using UnityEngine;
using UnityEngine.UI;
using RLSApi.Net.Models;
using RLSApi.Data;
using RLSApi;
using SteamApi;
using System;
using System.Collections.Generic;

public class SearchView : BaseUpdateView {
	[SerializeField]
	private GameObject _errorMessageDisplay;
	[SerializeField]
	private PlayerListView _playerListViewTemplate;
	[SerializeField]
	private Text _resultsTextfield;

	private RlsPlatform _platform;
	private string _searchText;
	private List<PlayerListView> _playerListViews = new List<PlayerListView>();

	public void SetQuery(RlsPlatform platform, string id) {
		_platform = platform;
		_searchText = id;
	}

	protected override void Init() {
		_playerListViewTemplate.gameObject.SetActive(false);
		_errorMessageDisplay.SetActive(false);
		base.Init();
	}

	protected override void OpenView() {
		switch (_platform) {
			case RlsPlatform.Any:
				SearchAnyUser(false, RlsPlatform.Any);
				break;
			case RlsPlatform.Steam:
				SearchSteamUser();
				break;
			case RlsPlatform.Ps4:
				SearchPS4User();
				break;
			case RlsPlatform.Xbox:
				SearchXboxUser();
				break;
		}
		base.OpenView();
	}

	private void SearchAnyUser(bool showError, RlsPlatform platform) {
		Loader.OnLoadStart();

		_playerListViews.ForEach(n => Destroy(n.gameObject));
		_playerListViews.Clear();

		var database = FindObjectOfType<PlayerDatabase>();

		SearchPlayer(_searchText, 0, (data) => {
			_errorMessageDisplay.SetActive(showError);
			Loader.OnLoadEnd();

			int numDifferentPlatform = 0;

			var players = data.Data.SortbyName(_searchText);
			foreach (var player in players) {
				database.StoreTempPlayer(player);

				if (platform != RlsPlatform.Any) {
					var playerPlatform = PlatformTool.GetPlatformData(player.Platform);
					if (platform != playerPlatform.Platform) {
						numDifferentPlatform++;
						continue;
					}
				}

				var playerListView = UITool.CreateField<PlayerListView>(_playerListViewTemplate);
				playerListView.Set(player.Convert());
				_playerListViews.Add(playerListView);
			}

			var total = data.TotalResults - numDifferentPlatform;
			if (total < data.MaxResultsPerPage) {
				_resultsTextfield.text = CopyDictionary.Get("NUMRESULTS", total.ToString());
			} else {
				_resultsTextfield.text = CopyDictionary.Get("NUMRESULTSLIMITREACHED", total.ToString(), data.MaxResultsPerPage.ToString());
			}
		}, (error) => {
			Loader.OnLoadEnd();
			//error
		});
	}

	private void SearchSteamUser() {
		Loader.OnLoadStart();
		SteamClient.ResolveVanityURL(_searchText, (steamData) => {
			if (steamData.Succes == 1) {
				PlayerTool.GetPlayer(RlsPlatform.Steam, steamData.Id, (playerData) => {
					Loader.OnLoadEnd();
					var app = FindObjectOfType<App>();
					app.SetPlayerView(playerData.Convert());
				}, (error) => {
					Loader.OnLoadEnd();
					//error handling
				});
			} else {
				SearchAnyUser(true, RlsPlatform.Steam);
			}
		}, (error) => {
			SearchAnyUser(true, RlsPlatform.Steam);
			//couldn't find steam name
		});
	}

	private void SearchPS4User() {
		GetUserOnPlatform(RlsPlatform.Ps4);
	}

	private void SearchXboxUser() {
		GetUserOnPlatform(RlsPlatform.Xbox);
	}

	private void GetUserOnPlatform(RlsPlatform platform) {
		Loader.OnLoadStart();
		PlayerTool.GetPlayer(_platform, _searchText, (data) => {
			Loader.OnLoadEnd();
			var app = FindObjectOfType<App>();
			app.SetPlayerView(data.Convert());
		}, (error) => {
			if (error.StatusCode == 404) {
				//couldn't find
				SearchAnyUser(true, platform);
			} else {
				Loader.OnLoadEnd();
				//error
			}
		});
	}

	private void SearchPlayer(string name, int pageNum, Action<PlayerSearchPage> onSuccess, Action<Error> onFail) {
		RLSClient.SearchPlayer(name, pageNum, onSuccess, onFail);
	}
}
