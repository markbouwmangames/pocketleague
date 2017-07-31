using UnityEngine;
using UnityEngine.UI;
using RLSApi.Data;
using System;
using System.Collections.Generic;

public class SeasonSelector : MonoBehaviour {
    public Action<RlsSeason> OnSeasonChanged;

	[SerializeField]
	private Button _buttonTemplate;
	private Dictionary<RlsSeason, Button> _buttons = new Dictionary<RlsSeason, Button>();


	void Awake() {
		_buttonTemplate.gameObject.SetActive(false);

		var rlsPlatforms = Enum.GetValues(typeof(RlsSeason));
		CreateSeasonButtons(rlsPlatforms);
	}

	private void CreateSeasonButtons(Array seasons) {
		var i = seasons.Length;
		while (--i > -1) {
			var season = (RlsSeason)(seasons.GetValue(i));

			var button = CreateButton(season);

			button.onClick.AddListener(() => {
				OnButtonPress(season);
			});

			_buttons.Add(season, button);
		}
	}

	public void SetSeasonButtons(RlsSeason[] seasons) {
		var seasonList = new List<RlsSeason>(seasons);
		foreach(var kvp in _buttons) {
			kvp.Value.gameObject.SetActive(seasonList.Contains(kvp.Key));
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
