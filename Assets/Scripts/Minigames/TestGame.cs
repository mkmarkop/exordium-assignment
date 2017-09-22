public class TestGame : AbstractGame {

	public override void InitializeGame () {
		Register (ScreenManager.Instance);
		gameTask.Initialize (6);
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
}
