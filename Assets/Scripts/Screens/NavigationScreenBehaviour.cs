using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationScreenBehaviour : ScreenBehaviour {

	public Image[] ColorStars;

	public Image[] MemoryStars;
	public Button MemoryButton;

	public void RefreshButtons() {
		int colorScore = DataStorage.Instance.GameProgress ("ColorMatch");
		ColorStars.ScoreToImages (colorScore);

		int memoryScore = DataStorage.Instance.GameProgress ("Memory");
		MemoryStars.ScoreToImages (memoryScore);

		MemoryButton.interactable = colorScore >= 0;
	}

	protected override void _onActivate() {
		RefreshButtons ();
	}

	protected override void _onDeactivate() {

	}
}
