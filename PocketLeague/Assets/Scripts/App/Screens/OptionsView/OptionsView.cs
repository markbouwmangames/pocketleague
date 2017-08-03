using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsView : BaseView {
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
