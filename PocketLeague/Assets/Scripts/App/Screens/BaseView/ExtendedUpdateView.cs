using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class ExtendedUpdateView : BaseUpdateView {
    [SerializeField]
    private RectTransform _loader;

    private ScrollRect _scrollRect;
    private bool _loading;

    protected override void Init() {
        _loader.gameObject.SetActive(false);
        _scrollRect = GetComponent<ScrollRect>();
        base.Init();
    }

    void Update() {
        if (isOpen == false) return;

		var content = _scrollRect.content;
        if (content.sizeDelta.y <= 0) return;

        var normalizedPosition = _scrollRect.verticalNormalizedPosition;

		if (_loading == false) {
            var height = content.sizeDelta.y;
            var percentage = _loader.sizeDelta.y / height;

            if (normalizedPosition > 1f + percentage) {
				if (Input.GetMouseButtonUp(0)) {
					_loading = true;
					_loader.gameObject.SetActive(true);
					UpdateView(() => {
						_loader.gameObject.SetActive(false);
						_loading = false;
						height = content.sizeDelta.y;
					});
				}
            }
        }

        if (_loading) {
			if (normalizedPosition > 1f) _scrollRect.verticalNormalizedPosition = 1f;
        }
    }

    protected override void OpenView() {
        base.OpenView();
        UpdateView();
    }

    protected abstract void UpdateView(Action onComplete = null);
}
