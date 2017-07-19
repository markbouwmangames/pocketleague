using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public enum Language {
	English = 0,
}

public class CopyDictionary {
	private static string KEYLOCATION = "_language";

	private static bool initialised = false;
	private static Language currentLanguage;

	private static Dictionary<string, string> copy;

	public static void SetLanguage(Language language) {
		currentLanguage = language;
		PlayerPrefs.SetInt(KEYLOCATION, ((int)(language)));

		string filePath = "Copy/" + language.ToString();
		Debug.Log(filePath);
		TextAsset targetFile = Resources.Load<TextAsset>(filePath);
		var json =  targetFile.text;
		copy = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
	}

	public static string Get(string key) {
		if (!initialised) Init();
		return copy[key];
	}

	public static void Get(string key, params string[] replace) {
		if (!initialised) Init();

		var str = copy[key];
		for (var i = 0; i < replace.Length; i++) {
			string stringReplace = "[" + i + "]";

		}
	}

	private static void Init() {
		initialised = true;

		var languageKey = PlayerPrefs.GetInt(KEYLOCATION, 0);
		var language = ((Language)(languageKey));

		SetLanguage(language);
	}
}
