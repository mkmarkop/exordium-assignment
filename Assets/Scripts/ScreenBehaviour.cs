using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class ScreenBehaviour : MonoBehaviour {

	public GameScreen screenID;

	private CanvasGroup canvasGroup;

	void Awake() {
		ScreenManager.Instance.RegisterScreen (this);
		canvasGroup = GetComponent<CanvasGroup> ();
		Deactivate ();
	}

	public void Activate() {
		canvasGroup.interactable = true;
		canvasGroup.alpha = 1f;
		gameObject.SetActive (true);
	}

	public void Deactivate() {
		canvasGroup.interactable = false;
		canvasGroup.alpha = 0f;
		gameObject.SetActive (false);
	}
}
