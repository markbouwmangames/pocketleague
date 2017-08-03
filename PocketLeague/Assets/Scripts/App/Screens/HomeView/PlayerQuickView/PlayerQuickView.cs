using UnityEngine;
using RLSApi.Net.Models;
using RLSApi.Data;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class PlayerQuickView : MonoBehaviour {
    public Action OnClick;
	
    [SerializeField]
    private RankDisplay _rankDisplay;
	[SerializeField]
	private Text _rankviewTitle;
    [SerializeField]
    private Button _button;

    void Awake() {
		_button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick() {
        if (OnClick != null) OnClick.Invoke();
    }

	public void Set(Player player) {
		_rankDisplay.Set(Constants.LatestSeason, player.CurrentSeason());
		_rankviewTitle.text = CopyDictionary.Get("RANKVIEWTITLE", player.DisplayName);
	}
}
