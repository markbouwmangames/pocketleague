using UnityEngine;

public class Loader : MonoBehaviour {
	private static CanvasGroup _canvasGroup;

	void Awake() {
		_canvasGroup = GetComponent<CanvasGroup>();
	}

	public static void OnLoadStart() {
		_canvasGroup.alpha = 1;
	}

	public static void OnLoadEnd() {
		_canvasGroup.alpha = 0;
	}
}
