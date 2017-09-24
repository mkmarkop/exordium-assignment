using UnityEngine;

public class TimerProxy : AbstractTimer {

	public AbstractTimer TrueTimer;

	public override void ResetTimer () {
		TrueTimer.ResetTimer ();
	}

	public override void Tick () {
		TrueTimer.Tick ();
	}

	public override void Register (ITimerTickListener tickListener) {
		TrueTimer.Register (tickListener);
	}

	public override void Unregister (ITimerTickListener tickListener) {
		TrueTimer.Unregister (tickListener);
	}

	public override void Register (ITimerDoneListener doneListener) {
		TrueTimer.Register (doneListener);
	}

	public override void Unregister (ITimerDoneListener doneListener) {
		TrueTimer.Unregister (doneListener);
	}

	public override void OnGameStateChange (GameState newState) {
		TrueTimer.OnGameStateChange (newState);
	}
}
