using UnityEngine;
using UnityEngine.UI;

public class SplashScreenBehaviour : ScreenBehaviour {

	public const string DESKTOP_TEXT =
		"Press any key or click to continue";

	public const string MOBILE_TEXT =
		"Tap to continue";
	
	public Text ContinueText;

	private bool _checkInput = false;

	protected override void _onActivate () {
		_checkInput = true;
	}

	protected override void _onDeactivate () {
		_checkInput = false;
	}

	void Start() {
		if (ContinueText != null) {
			#if UNITY_IOS || UNITY_ANDROID || UNITY_WP_8_1
				ContinueText.text = MOBILE_TEXT;
			#else
				ContinueText.text = DESKTOP_TEXT;
			#endif
		}
	}

	void Update() {
		if (_checkInput) {
			if (Input.anyKeyDown) {
				ScreenManager.Instance.ChangeScreen (GameScreen.MainMenuScreen);
			}
		}
	}
}
