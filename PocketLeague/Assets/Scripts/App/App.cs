using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour {
	[SerializeField]
	private HomeView _homeView;
	[SerializeField]
	private PlayerView _playerView;

	private BaseView _currentActive;
	private PlayerReferenceData _mainAccount;

	void Start() {
		_mainAccount = new PlayerReferenceData() {
			Platform = RLSApi.Data.RlsPlatform.Ps4,
			DisplayName = "Mefoz"
		};

		GoHome();
	}

	public void GoHome() {
		SetView(_homeView);
		_homeView.SetPlayer(_mainAccount);
	}

	private void SetView(BaseView view) {
		if (_currentActive != null) {
			_currentActive.SetEnabled(false);
		}

		view.SetEnabled(true);
		_currentActive = view;
	}
}
