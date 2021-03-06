﻿using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

public enum Language {
	EN = 0,
    NL = 1,
	ES = 2,
}

public class CopyDictionary {
	private static string KEYLOCATION = "_language";
    private static string KEYNOTFOUND = "KEYNOTFOUND";

	private static bool initialised = false;
	private static Language currentLanguage;

	private static Dictionary<string, string> copy;
	private static CopyTextfield[] copyTextfields;

	public static void SetLanguage(Language language) {
		SetLanguage(language, true);
	}

	private static void SetLanguage(Language language, bool update) {
		currentLanguage = language;
		PlayerPrefs.SetInt(KEYLOCATION, ((int)(language)));

		string filePath = "Copy/" + language.ToString();
		TextAsset targetFile = Resources.Load<TextAsset>(filePath);
		var json = targetFile.text;
		copy = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

		if(update) {
			foreach(var copyTextfield in copyTextfields) {
				copyTextfield.UpdateText();
			}
		}
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

    public static void FormatTime(DateTimeOffset dateTime, out string date, out string time) {
        string updateTimeS = dateTime.ToString("s");
        string updateTimeD = dateTime.ToString("d");

        var timeStrings = updateTimeS.Split('T');
        date = updateTimeD.Replace('/', '-');
        time = timeStrings[timeStrings.Length - 1];
    }

	private static void Init() {
		initialised = true;

		var languageKey = PlayerPrefs.GetInt(KEYLOCATION, 0);
		var language = ((Language)(languageKey));

		var app = GameObject.FindObjectOfType<App>();
		copyTextfields = app.GetComponentsInChildren<CopyTextfield>();

		SetLanguage(language, false);
	}
}
