using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class App : MonoBehaviour {
    [SerializeField]
    private HomeView _homeView;
    [SerializeField]
    private TrackedAccountsView _trackedAccountsView;
    [SerializeField]
    private OptionsView _optionsView;
	[SerializeField]
	private PlayerView _playerView;

	private BaseView _currentActive;
	private PlayerReferenceData _mainAccount;

	void Start() {
		_mainAccount = new PlayerReferenceData() {
			Platform = RLSApi.Data.RlsPlatform.Ps4,
			DisplayName = "Mefoz"
		};

        _homeView.SetMainPlayer(_mainAccount);
		SetHomeView();
	}

	public void SetHomeView() {
		SetView(_homeView);
	}

    public void SetTrackedAccountsView() {
        SetView(_trackedAccountsView);
    }

    public void SetOptionsView() {
      SetView(_optionsView);
    }

    public void SetPlayerView(PlayerReferenceData playerReference) {
		_playerView.SetPlayer(playerReference);
		SetView(_playerView);
    }

	private void SetView(BaseView view) {
		if (view == _currentActive) return;

		if (_currentActive != null) {
			_currentActive.SetEnabled(false);
		}

		view.SetEnabled(true);
		_currentActive = view;
	}
}
