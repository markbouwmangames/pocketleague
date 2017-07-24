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
        var button = UITool.CreateField<Button>(_buttonTemplate);
        var textfield = button.GetComponentInChildren<Text>();
		textfield.text = CopyDictionary.Get("SEASONNAME", ((int)(season)).ToString());

        return button;
	}
}
