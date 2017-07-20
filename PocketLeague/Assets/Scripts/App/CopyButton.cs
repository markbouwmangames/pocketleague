using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class CopyButton : MonoBehaviour {
	[SerializeField]
	private string _key;

	private Text _textfield;

	void Awake() {
		_textfield = GetComponent<Text>();
		UpdateText();
	}

	public void UpdateText() {
		_textfield.text = CopyDictionary.Get(_key);
	}
}
