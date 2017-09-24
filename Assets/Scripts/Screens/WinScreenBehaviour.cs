using UnityEngine;
using UnityEngine.UI;

public class WinScreenBehaviour : ScreenBehaviour {

	public Image[] ScoreStars;

	protected override void OnActivate () {
		int score = GameProxy.Instance.CalculateScore ();
		int n = Mathf.Min (score, ScoreStars.Length);

		for (int i = 0; i < n; i++) {
			ScoreStars [i].color = new Color (1f, 1f, 1f, 1f);
		}
	}

	protected override void OnDeactivate () {
		foreach (Image img in ScoreStars) {
			img.color = new Color(1f, 1f, 1f, 0f);
		}
	}
}
