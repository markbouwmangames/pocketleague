using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public enum Language {
	EN = 0,
    NL = 1,
}

public class CopyDictionary {
	private static string KEYLOCATION = "_language";
    private static string KEYNOTFOUND = "KEYNOTFOUND";

	private static bool initialised = false;
	private static Language currentLanguage;

	private static Dictionary<string, string> copy;

	public static void SetLanguage(Language language) {
		currentLanguage = language;
		PlayerPrefs.SetInt(KEYLOCATION, ((int)(language)));

		string filePath = "Copy/" + language.ToString();
		TextAsset targetFile = Resources.Load<TextAsset>(filePath);
		var json =  targetFile.text;
		copy = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
	}

	public static Language GetLanguage() {
		return currentLanguage;
	}

	public static string Get(string key) {
		if (!initialised) Init();
        
        var value = "";
        if (copy.TryGetValue(key, out value)) {
            return value;
        } else {
			Debug.LogWarning("Key not found: [" + key + "]");
            return KEYNOTFOUND;
        }
	}

	public static string Get(string key, params string[] replace) {
		if (!initialised) Init();

        var value = "";
        if (copy.TryGetValue(key, out value)) {
            var result = value;
            for (var i = 0; i < replace.Length; i++) {
                string stringToReplace = "[" + i + "]";
                result = result.Replace(stringToReplace, replace[i]);
            }

            return result;
        } else {
			Debug.LogWarning("Key not found: [" + key + "]");
			return KEYNOTFOUND;
        }
	}

	private static void Init() {
		initialised = true;

		var languageKey = PlayerPrefs.GetInt(KEYLOCATION, 0);
		var language = ((Language)(languageKey));

		SetLanguage(language);
	}
}
