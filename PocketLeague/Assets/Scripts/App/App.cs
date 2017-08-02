using UnityEngine;
using RLSApi.Net.Models;

public class App : MonoBehaviour {
    [SerializeField]
    private HomeView _homeView;
    [SerializeField]
    private TrackedAccountsView _trackedAccountsView;
	[SerializeField]
	private LeaderboardsView _leaderboardsView;
	[SerializeField]
    private OptionsView _optionsView;
	[SerializeField]
	private PlayerView _playerView;

	private BaseView _currentActive;
	private PlayerReferenceData _mainAccount;

	void Awake() {
		gameObject.AddComponent<PlayerDatabase>();
		gameObject.AddComponent<PlayerTool>();
		_mainAccount = new PlayerReferenceData(RLSApi.Data.RlsPlatform.Ps4, "Mefoz", "Mefoz");
	}


	void Start() {
        _homeView.SetMainPlayer(_mainAccount);
		//SetHomeView();
		SetLeaderboardsView();
	}

	public void SetHomeView() {
		SetView(_homeView);
	}

    public void SetTrackedAccountsView() {
        SetView(_trackedAccountsView);
    }

	public void SetLeaderboardsView() {
		SetView(_leaderboardsView);
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
