using UnityEngine;

public class TimerBehaviour : MonoBehaviour, ITimerPublisher, IGameListener {

	public float TimerLength = 20f;

	public event TimerTickHandler OnTimerTick;

	public event TimerDoneHandler OnTimerDone;

	private float _timeLeft;

	private GameState currentState = GameState.Inactive;

	void Start() {
		_timeLeft = TimerLength;
	}

	void Update() {
		if (currentState != GameState.Active)
			return;

		_timeLeft -= Time.deltaTime;
		_timeLeft = _timeLeft < 0 ? 0 : _timeLeft;
		if (OnTimerTick != null)
			OnTimerTick (_timeLeft);

		if (_timeLeft <= 0 && OnTimerDone != null)
			OnTimerDone ();
	}

	public void OnGameStateChange (GameState newState) {
		switch (newState) {
		case GameState.Inactive:
			_timeLeft = TimerLength;
			break;
		}

		currentState = newState;
	}

	public void Register (ITimerTickListener tickListener) {
		OnTimerTick += tickListener.OnTimerTick;
	}

	public void Unregister (ITimerTickListener tickListener) {
		OnTimerTick -= tickListener.OnTimerTick;
	}

	public void Register (ITimerDoneListener doneListener) {
		OnTimerDone += doneListener.OnTimerDone;
	}

	public void Unregister (ITimerDoneListener doneListener) {
		OnTimerDone -= doneListener.OnTimerDone;
	}
}
