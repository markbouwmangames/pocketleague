using UnityEngine;
using UnityEngine.UI;

public class StatDisplay : MonoBehaviour {
	[SerializeField]
	private Text _title;
	[SerializeField]
	private Text _stat;

	public void Set(string key, float num, string postFix = "") {
		_title.text = CopyDictionary.Get(key);
		_stat.text = num + postFix;
	}
}
