using UnityEngine;

public abstract class BaseView : MonoBehaviour {
	private CanvasGroup _canvasGroup;

	void Awake() {
		Init();
		_canvasGroup = GetComponent<CanvasGroup>();
		SetEnabled(false);
	}

	public void SetEnabled(bool value) {
		_canvasGroup.alpha = value ? 1 : 0;
		_canvasGroup.blocksRaycasts = value;
		_canvasGroup.interactable = value;

		if (value) UpdateView();
	}

	protected virtual void Init() {

	}
	protected abstract void UpdateView();
}
