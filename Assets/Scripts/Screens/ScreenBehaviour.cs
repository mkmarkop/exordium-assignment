using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenBehaviour : MonoBehaviour {

	public const float ANIMATION_TIME_INTERVAL = 0.0125f;

	public GameScreen ScreenID;

	protected CanvasGroup _canvasGroup;

	public delegate void ScreenExitHandler();
	public static event ScreenExitHandler onScreenDisappear;

	public delegate void ScreenEntranceHandler();
	public static event ScreenEntranceHandler onScreenAppear;

	void Awake() {
		ScreenManager.Instance.RegisterScreen (this);
		_canvasGroup = GetComponent<CanvasGroup> ();
		_canvasGroup.alpha = 0f;
		Deactivate ();
	}

	public IEnumerator TransitionIn() {
		for (float alpha = 0f; alpha <= 1f; alpha += 0.1f) {
			_canvasGroup.alpha = alpha;
			yield return new WaitForSeconds (ANIMATION_TIME_INTERVAL);
		}

		_canvasGroup.alpha = 1f;

		if (onScreenAppear != null)
			onScreenAppear ();
		
		yield return null;
	}

	public void Activate() {
		_canvasGroup.alpha = 1f;
		_canvasGroup.blocksRaycasts = true;
		_canvasGroup.interactable = true;
		_onActivate ();
	}

	protected virtual void _onActivate() {

	}

	public IEnumerator TransitionOut() {
		for (float alpha = 1f; alpha >= 0; alpha -= 0.1f) {
			_canvasGroup.alpha = alpha;
			yield return new WaitForSeconds (ANIMATION_TIME_INTERVAL);
		}

		_canvasGroup.alpha = 0f;

		if (onScreenDisappear != null)
			onScreenDisappear ();
		
		yield return null;
	}

	public void Deactivate() {
		_onDeactivate ();
		_canvasGroup.blocksRaycasts = false;
		_canvasGroup.interactable = false;
	}

	protected virtual void _onDeactivate() {

	}
}
