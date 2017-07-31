using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedAccountsView : BaseView {
	[SerializeField]
	private GameObject _addPlayerMessage;
	[SerializeField]
	private PlayerListView _playerListViewTemplate;

	private List<PlayerListView> _playerListViews = new List<PlayerListView>();

	protected override void Init() {
		base.Init();
		_playerListViewTemplate.gameObject.SetActive(false);
	}

	protected override void OpenView() {
        base.OpenView();

		var database = FindObjectOfType<PlayerDatabase>();
		var players = database.GetReferencedPlayers();
		Set(players);
    }

	private void Set(PlayerReferenceData[] players) {
		ClearPlayerListViews();

		for (var i = 0; i < players.Length; i++) {
			CreatePlayerListView(players[i]);
		}

		_addPlayerMessage.SetActive(players.Length == 0);
	}

	private void CreatePlayerListView(PlayerReferenceData playerReference) {
		var playerListView = UITool.CreateField<PlayerListView>(_playerListViewTemplate);
		playerListView.Set(playerReference);
		_playerListViews.Add(playerListView);
	}

	private void ClearPlayerListViews() {
		foreach (var v in _playerListViews) {
			Destroy(v.gameObject);
		}
		_playerListViews.Clear();
	}
}
