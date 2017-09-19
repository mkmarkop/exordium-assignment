using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenBehaviour : MonoBehaviour {

	public GameScreen screenID;

	protected CanvasGroup canvasGroup;

	void Awake() {
		ScreenManager.Instance.RegisterScreen (this);
		canvasGroup = GetComponent<CanvasGroup> ();
		Deactivate ();
	}

	public void Activate() {
		canvasGroup.interactable = true;
		canvasGroup.alpha = 1f;
		OnActivate ();
		gameObject.SetActive (true);
	}

	protected virtual void OnActivate() {

	}

	public void Deactivate() {
		OnDeactivate ();
		canvasGroup.interactable = false;
		canvasGroup.alpha = 0f;
		gameObject.SetActive (false);
	}

	protected virtual void OnDeactivate() {

	}
}
