using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenBehaviour : MonoBehaviour {

	public const float ANIMATION_TIME_INTERVAL = 0.0125f;

	public GameScreen screenID;

	protected CanvasGroup canvasGroup;

	public delegate void screenExitHandler();
	public static event screenExitHandler onScreenDisappear;

	public delegate void screenEntranceHandler();
	public static event screenEntranceHandler onScreenAppear;

	void Awake() {
		ScreenManager.Instance.RegisterScreen (this);
		canvasGroup = GetComponent<CanvasGroup> ();
		canvasGroup.alpha = 0f;
		Deactivate ();
	}

	public IEnumerator TransitionIn() {
		for (float alpha = 0f; alpha <= 1f; alpha += 0.1f) {
			canvasGroup.alpha = alpha;
			yield return new WaitForSeconds (ANIMATION_TIME_INTERVAL);
		}

		canvasGroup.alpha = 1f;

		if (onScreenAppear != null)
			onScreenAppear ();
		
		yield return null;
	}

	public void Activate() {
		canvasGroup.alpha = 1f;
		canvasGroup.blocksRaycasts = true;
		canvasGroup.interactable = true;
		OnActivate ();
	}

	protected virtual void OnActivate() {

	}

	public IEnumerator TransitionOut() {
		for (float alpha = 1f; alpha >= 0; alpha -= 0.1f) {
			canvasGroup.alpha = alpha;
			yield return new WaitForSeconds (ANIMATION_TIME_INTERVAL);
		}

		canvasGroup.alpha = 0f;

		if (onScreenDisappear != null)
			onScreenDisappear ();
		
		yield return null;
	}

	public void Deactivate() {
		OnDeactivate ();
		canvasGroup.blocksRaycasts = false;
		canvasGroup.interactable = false;
	}

	protected virtual void OnDeactivate() {

	}
}
