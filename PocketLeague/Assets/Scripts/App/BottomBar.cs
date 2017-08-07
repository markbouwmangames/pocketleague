using UnityEngine;

public class BottomBar : MonoBehaviour {
	public void OpenHomeView() {
		var app = FindObjectOfType<App>();
		if (app.CurrentView is HomeView) return;
		app.SetHomeView();
	}

	public void OpenTrackedAccountsView() {
		var app = FindObjectOfType<App>();
		if (app.CurrentView is TrackedAccountsView) return;
		app.SetTrackedAccountsView();
	}

	public void OpenLeaderboardsView() {
		var app = FindObjectOfType<App>();
		if (app.CurrentView is LeaderboardsView) return;
		app.SetLeaderboardsView();
	}

	public void OpenOptionsView() {
		var app = FindObjectOfType<App>();
		if (app.CurrentView is OptionsView) return;
		app.SetOptionsView();
	}
}
