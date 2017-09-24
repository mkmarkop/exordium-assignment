using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour, IGameListener {

	enum ScreenManagerState {
		active,
		transitioning
	}

	protected ScreenManager() {}

	private static ScreenManager _instance;
	private static object _lock = new object();

	public static ScreenManager Instance {
		get {
			lock (_lock) {
				if (_instance == null) {
					_instance = (ScreenManager)FindObjectOfType (typeof(ScreenManager));
					if (_instance == null) {
						GameObject scrManagerObj = new GameObject ();
						scrManagerObj.name = "ScreenManager";
						_instance = scrManagerObj.AddComponent<ScreenManager> ();
						DontDestroyOnLoad (_instance);
					}
				}

				return _instance;
			}
		}
	}

	private GameScreen _previousScreenID = GameScreen.NoScreen;
	// For testing purposes
	private GameScreen _currentScreenID = GameScreen.GameStartScreen;
	private GameScreen _nextScreenID = GameScreen.NoScreen;
	private Dictionary<GameScreen, ScreenBehaviour> screenMapping =
		new Dictionary<GameScreen, ScreenBehaviour> ();

	private ScreenManagerState currentState;
	public GameScreen StartScreenID;

	void screenExitHandler() {
		ScreenBehaviour nextScreen = screenMapping [_nextScreenID];
		StartCoroutine (nextScreen.TransitionIn ());
	}

	void screenEntranceHandler() {
		currentState = ScreenManagerState.active;
		_previousScreenID = _currentScreenID;
		_currentScreenID = _nextScreenID;
		currentScreen ().Activate ();
	}

	void Awake() {
		ScreenBehaviour.onScreenAppear += screenEntranceHandler;
		ScreenBehaviour.onScreenDisappear += screenExitHandler;
	}

	// Use this for initialization
	void Start () {
		_currentScreenID = StartScreenID;
		currentScreen ().Activate ();
		currentState = ScreenManagerState.active;
	}

	ScreenBehaviour currentScreen() {
		return screenMapping [_currentScreenID];
	}

	public void RegisterScreen(ScreenBehaviour screenObj) {
		screenMapping.Add (screenObj.screenID, screenObj);
	}

	public void ChangeScreen(ScreenBehaviour screenObj) {
		if (screenObj != null) {
			ChangeScreen (screenObj.screenID);
		}
	}

	public void ChangeScreen(GameScreen newScreenID) {
		if (!screenMapping.ContainsKey (newScreenID))
			return;

		if (screenMapping [newScreenID] == null)
			return;

		if (newScreenID == GameScreen.NoScreen)
			return;

		_nextScreenID = newScreenID;

		if (_currentScreenID == GameScreen.NoScreen) {
			screenEntranceHandler ();
		} else {
			currentScreen ().Deactivate ();
			currentState = ScreenManagerState.transitioning;
			StartCoroutine (currentScreen ().TransitionOut ());
		}
	}

	public void GoToPrevious() {
		if (_previousScreenID == GameScreen.NoScreen)
			return;

		ChangeScreen (_previousScreenID);
	}

	public void OnGameStateChange (GameState newState) {
		switch (newState) {
		case GameState.Active:
			ChangeScreen (GameScreen.GamePlayingScreen);
			break;

		case GameState.Inactive:
			ChangeScreen (GameScreen.GameStartScreen);
			break;

		case GameState.Lost:
			ChangeScreen (GameScreen.GameLostScreen);
			break;

		case GameState.Won:
			ChangeScreen (GameScreen.GameWonScreen);
			break;

		case GameState.Paused:
			ChangeScreen (GameScreen.GamePausedScreen);
			break;
		}
	}

	public void Shutdown() {
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}
