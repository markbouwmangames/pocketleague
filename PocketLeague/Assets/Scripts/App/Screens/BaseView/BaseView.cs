using UnityEngine;

public abstract class BaseView : MonoBehaviour {
	private CanvasGroup _canvasGroup;
    protected bool isOpen;

	void Awake() {
		Init();
		_canvasGroup = GetComponent<CanvasGroup>();
		SetEnabled(false);
	}

	public void SetEnabled(bool value) {
		if (value) gameObject.SetActive(value);

		_canvasGroup.alpha = value ? 1 : 0;
		_canvasGroup.blocksRaycasts = value;
		_canvasGroup.interactable = value;

        if (value) OpenView();
        else if(isOpen) CloseView();
		if (!value) gameObject.SetActive(value);
		
		isOpen = value;
	}

	protected virtual void Init() {

	}

    protected virtual void OpenView() {

    }

    protected virtual void CloseView() {
		Loader.OnLoadEnd();
	}
}
