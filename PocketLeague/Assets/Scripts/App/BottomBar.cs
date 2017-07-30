using UnityEngine;

public class BottomBar : MonoBehaviour {
    [SerializeField]
    private App _app;

    public void OnHomePressed() {
        _app.SetHomeView();
    }

    public void OnTrackedAccountsPressed() {
        _app.SetTrackedAccountsView();
    }

    public void OnOptionsPressed() {
        _app.SetOptionsView();
    }
}
