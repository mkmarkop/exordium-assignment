using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : Singleton<ScreenManager>, IGameListener {

	enum ScreenManagerState {
		Active,
		Transitioning
	}

	protected ScreenManager() {}

	private GameScreen _previousScreenID = GameScreen.NoScreen;
	// For testing purposes
	private GameScreen _currentScreenID = GameScreen.GameStartScreen;
	private GameScreen _nextScreenID = GameScreen.NoScreen;
	private Dictionary<GameScreen, ScreenBehaviour> _screenMapping =
		new Dictionary<GameScreen, ScreenBehaviour> ();

	private ScreenManagerState _currentState;
	public GameScreen StartScreenID;

	void screenExitHandler() {
		ScreenBehaviour nextScreen = _screenMapping [_nextScreenID];
		StartCoroutine (nextScreen.TransitionIn ());
	}

	void screenEntranceHandler() {
		_currentState = ScreenManagerState.Active;
		_previousScreenID = _currentScreenID;
		_currentScreenID = _nextScreenID;
		_currentScreen ().Activate ();
	}

	void Awake() {
		ScreenBehaviour.onScreenAppear += screenEntranceHandler;
		ScreenBehaviour.onScreenDisappear += screenExitHandler;
	}

	// Use this for initialization
	void Start () {
		_currentScreenID = StartScreenID;
		_currentScreen ().Activate ();
		_currentState = ScreenManagerState.Active;
	}

	ScreenBehaviour _currentScreen() {
		return _screenMapping [_currentScreenID];
	}

	public void RegisterScreen(ScreenBehaviour screenObj) {
		_screenMapping.Add (screenObj.ScreenID, screenObj);
	}

	public void ChangeScreen(ScreenBehaviour screenObj) {
		if (screenObj != null) {
			ChangeScreen (screenObj.ScreenID);
		}
	}

	public void ChangeScreen(GameScreen newScreenID) {
		if (!_screenMapping.ContainsKey (newScreenID))
			return;

		if (_screenMapping [newScreenID] == null)
			return;

		if (newScreenID == GameScreen.NoScreen)
			return;

		_nextScreenID = newScreenID;

		if (_currentScreenID == GameScreen.NoScreen) {
			screenEntranceHandler ();
		} else {
			_currentScreen ().Deactivate ();
			_currentState = ScreenManagerState.Transitioning;
			StartCoroutine (_currentScreen ().TransitionOut ());
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
