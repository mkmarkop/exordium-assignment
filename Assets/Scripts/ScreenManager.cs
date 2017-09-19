﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {

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

	private GameScreen _currentScreenID = GameScreen.SplashScreen;
	private GameScreen _nextScreenID = GameScreen.NoScreen;
	private Dictionary<GameScreen, ScreenBehaviour> screenMapping =
		new Dictionary<GameScreen, ScreenBehaviour> ();

	private ScreenManagerState currentState;

	void screenExitHandler() {
		ScreenBehaviour nextScreen = screenMapping [_nextScreenID];
		StartCoroutine (nextScreen.TransitionIn ());
	}

	void screenEntranceHandler() {
		currentState = ScreenManagerState.active;
		_currentScreenID = _nextScreenID;
		currentScreen ().Activate ();
	}

	void Awake() {
		ScreenBehaviour.onScreenAppear += screenEntranceHandler;
		ScreenBehaviour.onScreenDisappear += screenExitHandler;
	}

	// Use this for initialization
	void Start () {
		currentScreen ().Activate ();
		currentState = ScreenManagerState.active;
	}

	ScreenBehaviour currentScreen() {
		return screenMapping [_currentScreenID];
	}

	public void RegisterScreen(ScreenBehaviour screenObj) {
		screenMapping.Add (screenObj.screenID, screenObj);
	}

	public bool ChangeScreen(GameScreen newScreenID) {
		if (!screenMapping.ContainsKey (newScreenID))
			return false;

		if (screenMapping [newScreenID] == null)
			return false;

		if (newScreenID == GameScreen.NoScreen)
			return false;

		_nextScreenID = newScreenID;

		if (_currentScreenID == GameScreen.NoScreen) {
			screenEntranceHandler ();
		} else {
			currentScreen ().Deactivate ();
			currentState = ScreenManagerState.transitioning;
			StartCoroutine (currentScreen ().TransitionOut ());
		}

		return true;
	}
}
