using UnityEngine;
using UnityEngine.UI;
using RLSApi.Data;
using System;

public class SeasonSelector : MonoBehaviour {
    public Action<RlsSeason> OnSeasonChanged;

	[SerializeField]
	private Button _buttonTemplate;
	private Button[] _buttons;


	void Awake() {
		_buttonTemplate.gameObject.SetActive(false);
	}

	public void SetSeasonButtons(RlsSeason[] seasons) {
		_buttons = new Button[seasons.Length];

		var i = seasons.Length;
		while(--i > -1) {
			var season = seasons[i];

			var button = CreateButton(season);
			_buttons[i] = button;

			button.onClick.AddListener(() => {
				OnButtonPress(season);
			});
		}
	}

	private void OnButtonPress(RlsSeason season) {
        if (OnSeasonChanged != null) OnSeasonChanged.Invoke(season);
	}

	private Button CreateButton(RlsSeason season) {
		var newGO = GameObject.Instantiate(_buttonTemplate.gameObject);

		newGO.transform.SetParent(_buttonTemplate.transform.parent);
		newGO.transform.localScale = _buttonTemplate.transform.localScale;

		newGO.SetActive(true);

		var textfield = newGO.GetComponentInChildren<Text>();
		textfield.text = CopyDictionary.Get("SEASONNAME", ((int)(season)).ToString());

		return newGO.GetComponent<Button>();
	}
}
