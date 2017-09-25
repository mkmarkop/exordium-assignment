using UnityEngine;

public class TimerBehaviour : AbstractTimer {

	private float _timerLength;

	public float TimerLength {
		get {
			return _timerLength;
		}
		set {
			_timerLength = value;
			ResetTimer ();
		}
	}

	public event TimerTickHandler OnTimerTick;

	public event TimerDoneHandler OnTimerDone;

	private float _timeLeft;

	private GameState currentState = GameState.Inactive;

	void Start() {
		ResetTimer ();
	}

	void Update() {
		if (currentState != GameState.Active)
			return;

		Tick ();
	}

	public override void ResetTimer () {
		_timeLeft = TimerLength;
	}

	public override void Tick () {
		_timeLeft -= Time.deltaTime;
		_timeLeft = _timeLeft < 0 ? 0 : _timeLeft;
		if (OnTimerTick != null)
			OnTimerTick (_timeLeft);

		if (_timeLeft <= 0 && OnTimerDone != null)
			OnTimerDone ();
	}

	public override void OnGameStateChange (GameState newState) {
		switch (newState) {
		case GameState.Inactive:
			_timeLeft = TimerLength;
			break;
		}

		currentState = newState;
	}

	public override void Register (ITimerTickListener tickListener) {
		OnTimerTick += tickListener.OnTimerTick;
	}

	public override void Unregister (ITimerTickListener tickListener) {
		OnTimerTick -= tickListener.OnTimerTick;
	}

	public override void Register (ITimerDoneListener doneListener) {
		OnTimerDone += doneListener.OnTimerDone;
	}

	public override void Unregister (ITimerDoneListener doneListener) {
		OnTimerDone -= doneListener.OnTimerDone;
	}
}
