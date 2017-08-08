using UnityEngine;
using UnityEngine.EventSystems;
using TheNextFlow.UnityPlugins;
using UnityEngine.UI;

public class EditableListItem : MonoBehaviour {
	[SerializeField]
	private RectTransform _rectTransform;
	[SerializeField]
	private RectTransform _content;
	[SerializeField]
	private PlayerListView _playerListView;
	[SerializeField]
	private TrackedAccountsView _trackedAccountsView;
	[SerializeField]
	private Button _removeButton;

	private bool _dragging = false;
	private Vector3 _mousePos;

	private Vector2 _anchor;
	private float minDistance = 70f;
	private float maxDistance = 140f;

	void Awake() {
		_anchor = _content.anchoredPosition;
		_removeButton.onClick.AddListener(() => {

#if UNITY_EDITOR
			_trackedAccountsView.Remove(_playerListView.Player);
#else
		MobileNativePopups.OpenAlertDialog(
			CopyDictionary.Get("REMOVEPLAYER_TITLE"), CopyDictionary.Get("REMOVEPLAYER_MESSAGE", _playerListView.Player.DisplayName),
			CopyDictionary.Get("REMOVEPLAYER_OK"), CopyDictionary.Get("REMOVEPLAYER_CANCEL"),
		() => { _trackedAccountsView.Remove(_playerListView.Player); }, () => { _content.anchoredPosition = _anchor; });
#endif
		});
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			if (IsInRect(Input.mousePosition)) {
				_dragging = true;
				_mousePos = Input.mousePosition;
			} else {
				_content.anchoredPosition = _anchor;
			}
		}

		if (Input.GetMouseButton(0)) {
			if (!_dragging) return;

			var offset = Input.mousePosition - _mousePos;
			var xOffset = offset.x;
			_mousePos = Input.mousePosition;

			var newPos = _content.anchoredPosition;
			newPos.x = Mathf.Clamp(newPos.x + xOffset, _anchor.x - maxDistance, _anchor.x);
			_content.anchoredPosition = newPos;
		}

		if (Input.GetMouseButtonUp(0)) {
			if (!_dragging) return;
			_dragging = false;

			var pos = _content.anchoredPosition;
			if (pos.x < _anchor.x - minDistance) {
				pos.x = _anchor.x - maxDistance;
				_content.anchoredPosition = pos;
			} else {
				_content.anchoredPosition = _anchor;
			}
		}
	}

	private bool IsInRect(Vector2 pos) {
		Vector3[] corners = new Vector3[4];
		_rectTransform.GetWorldCorners(corners);
		Rect newRect = new Rect(corners[0], corners[2] - corners[0]);
		return newRect.Contains(pos);
	}
}
