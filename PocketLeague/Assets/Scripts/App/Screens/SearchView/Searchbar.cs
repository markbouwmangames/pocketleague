using UnityEngine;
using UnityEngine.UI;

public class Searchbar : MonoBehaviour {
	[SerializeField]
	private InputField _inputField;
	[SerializeField]
	private PlatformDropdown _platformDropdown;

	public void Search() {
		var platform = _platformDropdown.GetValue();
		var id = _inputField.text;

		var app = FindObjectOfType<App>();
		app.SetSearchView(platform, id);
	}
}
