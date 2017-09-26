using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStorage : Singleton<DataStorage> {

	protected DataStorage() {}

	public AbstractGame[] GamePrefabs;

	private Dictionary<string, int> _gameProgress =
		new Dictionary<string, int>();

	// Use this for initialization
	void Start () {
		foreach (AbstractGame game in GamePrefabs) {
			_gameProgress.Add (game.GameID, -1);
		}
	}

	public bool IsCompleted(string game) {
		if (!_gameProgress.ContainsKey (game))
			return false;

		return _gameProgress [game] >= 0;
	}

	public int GameProgress(string game) {
		if (!_gameProgress.ContainsKey (game))
			return -1;

		return _gameProgress [game];
	}

	public void SaveProgress(string game, int progress) {
		if (!_gameProgress.ContainsKey (game))
			return;

		if (_gameProgress [game] < progress)
			_gameProgress [game] = progress;
	}

	public void UnlockAll() {
		List<string> keys = new List<string> (_gameProgress.Keys);

		foreach (string game in keys) {
			if (_gameProgress [game] < 0)
				_gameProgress [game] = 0;
		}
	}

	public void ResetProgress() {
		List<string> keys = new List<string> (_gameProgress.Keys);

		foreach (string game in keys) {
			_gameProgress [game] = -1;
		}
	}
}
