using UnityEngine;

public class TestGame : AbstractGame {

	public override void InitializeGame () {
		Register (ScreenManager.Instance);
		gameTask.Initialize (Random.Range(6, 10));
	}

	protected override void onStateChange (GameState newState) {
		switch (newState) {
		case GameState.Active:
			break;

		case GameState.Inactive:
			Unregister (ScreenManager.Instance);
			break;

		case GameState.Lost:
			break;

		case GameState.Won:
			break;

		case GameState.Paused:
			break;
		}
	}

	protected override bool isValidTransition (GameState newState) {
		return true;
	}

	public override void UpdateGame () {
		//
	}

	public override int CalculateScore () {
		return Mathf.RoundToInt(((float)gameTask.GoalStepsTaken / (float)gameTask.GoalStepsRequired) * 3);
	}
}
