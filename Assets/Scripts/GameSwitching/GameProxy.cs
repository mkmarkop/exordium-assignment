using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimerProxy))]
[RequireComponent(typeof(GameTaskProxy))]
//[RequireComponent(typeof(GameHelpProxy))]
//how to calculate score?
public class GameProxy : MonoBehaviour, IGameManager,
IMinigame, IGamePublisher {

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

	public AbstractGame CurrentGame;
	private TimerProxy _timerProxy;
	private GameTaskProxy _gameTaskProxy;

	public delegate void GameLoadHandler();
	public event GameLoadHandler OnGameLoad;

	public delegate void GameExitHandler();
	public event GameExitHandler OnGameExit;

	void Start() {
		_timerProxy = GetComponent<TimerProxy> ();
		_gameTaskProxy = GetComponent<GameTaskProxy> ();
	}

	private void clear() {
		foreach (Transform child in transform) {
			Destroy (child.gameObject);
		}
	}

	public void LoadGame (AbstractGame game) {
		CurrentGame = Instantiate<AbstractGame>(game);
		CurrentGame.transform.parent = this.transform;
		_timerProxy.TrueTimer = CurrentGame.GetComponent<TimerBehaviour> ();
		_gameTaskProxy.TrueTask = CurrentGame.GetComponent<GameTaskBehaviour> ();

		if (OnGameLoad != null)
			OnGameLoad ();
	}

	public void ExitGame () {
		if (OnGameExit != null)
			OnGameExit ();

		_timerProxy.TrueTimer = null;
		_gameTaskProxy.TrueTask = null;

		clear ();
	}

	public void Register (IGameManagerListener managerListener) {
		OnGameLoad += managerListener.OnGameLoad;
		OnGameExit += managerListener.OnGameExit;
	}

	public void Unregister (IGameManagerListener managerListener) {
		OnGameLoad -= managerListener.OnGameLoad;
		OnGameExit -= managerListener.OnGameExit;
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

	public int CalculateScore() {
		return CurrentGame.CalculateScore ();
	}
}
