using UnityEngine;
using UnityEngine.UI;
using System;

public class LeaderboardToggle : MonoBehaviour {
	public Action OnClick;

	[SerializeField]
	private Text _textfield;

	void Awake() {
		var toggle = GetComponent<Toggle>();
		toggle.onValueChanged.AddListener(OnToggleValueChanged);
	}

	public void SetText(string value) {
		var text = CopyDictionary.Get(value);
		_textfield.text = text;
	}

	private void OnToggleValueChanged(bool value) {
		var toggle = GetComponent<Toggle>();
		if (toggle.isOn) {
			if (OnClick != null) OnClick();
		}
	}
}
