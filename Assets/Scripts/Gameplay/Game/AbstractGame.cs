using UnityEngine;

[RequireComponent(typeof(TimerBehaviour))]
[RequireComponent(typeof(GameTaskBehaviour))]
public abstract class AbstractGame : MonoBehaviour, IMinigame,
IGamePublisher, ITimerDoneListener, IProgressCompletionListener {

	public string GameID;

	[TextArea(3, 50)]
	public string GameInstructions;

	public event GameStateHandler OnGameStateChange;

	public GameState CurrentState { get; private set; }

	protected TimerBehaviour _gameTimer;

	protected GameTaskBehaviour _gameTask;

	void Start() {
		CurrentState = GameState.Inactive;

		_gameTimer = GetComponent<TimerBehaviour> ();
		if (_gameTimer != null) {
			_gameTimer.Register (this);
			Register (_gameTimer);
		}

		_gameTask = GetComponent<GameTaskBehaviour> ();
		if (_gameTask != null)
			_gameTask.Register (this);
	}

	public abstract void InitializeGame ();

	public void StartGame () {
		InitializeGame ();
		_changeState (GameState.Active);
	}

	public void ResetGame () {
		_changeState (GameState.Inactive);
	}

	public void PauseGame () {
		_changeState (GameState.Paused);
	}

	public void ResumeGame () {
		_changeState (GameState.Active);
	}

	public void Register (IGameListener gameListener) {
		OnGameStateChange += gameListener.OnGameStateChange;
	}

	public void Unregister (IGameListener gameListener) {
		OnGameStateChange -= gameListener.OnGameStateChange;
	}

	public void OnTimerDone () {
		_changeState (GameState.Lost);
	}

	public void OnProgressComplete () {
		_changeState (GameState.Won);
	}

	protected abstract void _onStateChange(GameState newState);

	protected abstract bool _isValidTransition(GameState newState);

	protected void _changeState(GameState newState) {
		if (newState == CurrentState)
			return;

		if (!_isValidTransition (newState))
			return;

		CurrentState = newState;

		if (OnGameStateChange != null)
			OnGameStateChange (CurrentState);

		_onStateChange (CurrentState);
	}

	void Update() {
		if (CurrentState != GameState.Active)
			return;

		UpdateGame ();
	}

	public abstract void UpdateGame();

	public abstract int CalculateScore();
}
