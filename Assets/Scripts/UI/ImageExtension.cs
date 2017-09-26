using UnityEngine;
using UnityEngine.UI;

public static class ImageExtension {

	public static void ScoreToImages(this Image[] array, int score) {
		int n = Mathf.Min (score, array.Length);

		for (int i = 0; i < array.Length; i++) {
			array [i].color = new Color (1f, 1f, 1f, 0f);
		}

		for (int i = 0; i < n; i++) {
			array [i].color = new Color (1f, 1f, 1f, 1f);
		}
	}
}
