using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DataStorage : Singleton<DataStorage> {

	private const string SAVE_FILENAME = "minigames.proggress";

	[System.Serializable]
	class PlayerProgress {
		
		public string[] GameIDs;

		public int[] GameScores;

		public PlayerProgress(int noGames) {
			GameIDs = new string[noGames];
			GameScores = new int[noGames];
		}
	}

	protected DataStorage() {}

	public AbstractGame[] GamePrefabs;

	private Dictionary<string, int> _gameProgress =
		new Dictionary<string, int>();

	// Use this for initialization
	void Start () {
		foreach (AbstractGame game in GamePrefabs) {
			_gameProgress.Add (game.GameID, -1);
		}

		_load ();
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

		if (_gameProgress [game] < progress) {
			_gameProgress [game] = progress;
			_save ();
		}
	}

	public void UnlockAll() {
		List<string> keys = new List<string> (_gameProgress.Keys);

		foreach (string game in keys) {
			if (_gameProgress [game] < 0)
				_gameProgress [game] = 0;
		}

		_save ();
	}

	public void ResetProgress() {
		List<string> keys = new List<string> (_gameProgress.Keys);

		foreach (string game in keys) {
			_gameProgress [game] = -1;
		}

		_save ();
	}

	private PlayerProgress _takeSnasphot() {
		PlayerProgress progg = new PlayerProgress (_gameProgress.Keys.Count);

		List<string> keys = new List<string> (_gameProgress.Keys);
		progg.GameIDs = keys.ToArray();

		for (int i = 0; i < progg.GameIDs.Length; i++) {
			progg.GameScores [i] = _gameProgress[progg.GameIDs [i]];
		}

		return progg;
	}

	private void _save() {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/" + SAVE_FILENAME);
		bf.Serialize (file, _takeSnasphot());
		file.Close ();
	}

	private void _loadSnapshot(PlayerProgress progg) {
		if (progg.GameIDs.Length != progg.GameScores.Length)
			return;

		for (int i = 0; i < progg.GameIDs.Length; i++) {
			_gameProgress [progg.GameIDs [i]] = progg.GameScores [i];
		}
	}

	private void _load() {
		if (File.Exists(Application.persistentDataPath + "/" + SAVE_FILENAME)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/" + SAVE_FILENAME,
				                  FileMode.OpenOrCreate);
			PlayerProgress progg = (PlayerProgress)bf.Deserialize (file);
			file.Close ();
			_loadSnapshot (progg);
		}
	}
}
