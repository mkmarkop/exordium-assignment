using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimerProxy))]
[RequireComponent(typeof(GameTaskProxy))]
public class GameProxy : MonoBehaviour, IMinigame, IGamePublisher {

	public AbstractGame CurrentGame;

	private static GameProxy _instance;
	private static object _lock = new object ();
	public static GameProxy Instance {
		get {
			lock (_lock) {
				if (_instance == null) {
					_instance = (GameProxy)FindObjectOfType (typeof(GameProxy));
					if (_instance == null) {
						GameObject gameProxy = new GameObject ();
						gameProxy.name = "GameProxy";
						_instance = gameProxy.AddComponent<GameProxy> ();
						DontDestroyOnLoad (_instance);
					}
				}
			}

			return _instance;
		}
	}

	public void InitializeGame () {
		CurrentGame.InitializeGame ();
	}

	public void StartGame () {
		CurrentGame.StartGame ();
	}

	public void ResetGame () {
		CurrentGame.ResetGame ();
	}

	public void PauseGame () {
		CurrentGame.PauseGame ();
	}

	public void ResumeGame () {
		CurrentGame.ResumeGame ();
	}

	public void Register (IGameListener gameListener) {
		CurrentGame.Register (gameListener);
	}

	public void Unregister (IGameListener gameListener) {
		CurrentGame.Unregister (gameListener);
	}
}
