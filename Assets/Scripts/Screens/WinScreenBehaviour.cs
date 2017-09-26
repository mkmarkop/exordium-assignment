using UnityEngine;
using UnityEngine.UI;

public class WinScreenBehaviour : ScreenBehaviour {

	public Image[] ScoreStars;

	protected override void _onActivate () {
		int score = GameProxy.Instance.CalculateScore ();
		ScoreStars.ScoreToImages (score);

		DataStorage.Instance.SaveProgress (
			GameProxy.Instance.CurrentGame.GameID,
			score);
	}

	protected override void _onDeactivate () {
		foreach (Image img in ScoreStars) {
			img.color = new Color(1f, 1f, 1f, 0f);
		}
	}
}
