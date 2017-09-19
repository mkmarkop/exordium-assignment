using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour {

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
	private Dictionary<GameScreen, ScreenBehaviour> screenMapping =
		new Dictionary<GameScreen, ScreenBehaviour> ();

	// Use this for initialization
	void Start () {
		currentScreen ().Activate ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	ScreenBehaviour currentScreen() {
		return screenMapping [_currentScreenID];
	}

	public void RegisterScreen(ScreenBehaviour screenObj) {
		screenMapping.Add (screenObj.screenID, screenObj);
	}

	public bool ChangeScreen(GameScreen newScreenID) {
		Debug.Log ("Trying to change to: " + newScreenID);
		if (!screenMapping.ContainsKey (newScreenID))
			return false;

		if (screenMapping [newScreenID] == null)
			return false;

		currentScreen ().Deactivate ();
		_currentScreenID = newScreenID;
		currentScreen ().Activate ();

		return true;
	}

}
