using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class OptionsView : BaseView {
	[SerializeField]
	private Toggle _toggleTemplate;

	protected override void Init() {
		base.Init();

		_toggleTemplate.gameObject.SetActive(false);
		var languages = Enum.GetValues(typeof(Language));
		CreateToggles(languages);
	}

	private void CreateToggles(Array languages) {
		for(var i = 0; i < languages.Length; i++) {
			var language = (Language)(languages.GetValue(i));

			var newToggle = UITool.CreateField<Toggle>(_toggleTemplate);
			newToggle.isOn = (i == 0);

			var textfield = newToggle.GetComponentInChildren<Text>();
			textfield.text = CopyDictionary.Get(language.ToString().ToUpper());
			newToggle.onValueChanged.AddListener((value) => {
				if (value) {
					OnToggleChanged(language);
				}
			});
		}
	}

	private void OnToggleChanged(Language value) {
		CopyDictionary.SetLanguage(value);
	}

	protected override void OpenView() {
        base.OpenView();
    }

	public void ResetMain() {

	}

	public void GoToPsyonix() {
		Application.OpenURL(Constants.PsyonixUrl);
	}

	public void GoToRLS() {
		Application.OpenURL(Constants.RLSUrl);
	}
}
