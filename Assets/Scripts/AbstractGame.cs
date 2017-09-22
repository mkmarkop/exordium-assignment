using UnityEngine;

public abstract class AbstractGame : MonoBehaviour, IMinigame,
IGamePublisher, ITimerDoneListener, IProgressCompletionListener {

	public event GameStateHandler OnGameStateChange;

	public GameState CurrentState { get; private set; }

	protected TimerBehaviour gameTimer;

	protected GameTaskBehaviour gameTask;

	void Start() {
		CurrentState = GameState.Inactive;

		gameTimer = GetComponent<TimerBehaviour> ();
		if (gameTimer != null) {
			gameTimer.Register (this);
			Register (gameTimer);
		}

		gameTask = GetComponent<GameTaskBehaviour> ();
		if (gameTask != null)
			gameTask.Register (this);
	}

	public abstract void InitializeGame ();

	public void StartGame () {
		InitializeGame ();
		changeState (GameState.Active);
	}

	public void ResetGame () {
		changeState (GameState.Inactive);
	}

	public void PauseGame () {
		changeState (GameState.Paused);
	}

	public void ResumeGame () {
		changeState (GameState.Active);
	}

	public void Register (IGameListener gameListener) {
		OnGameStateChange += gameListener.OnGameStateChange;
	}

	public void Unregister (IGameListener gameListener) {
		OnGameStateChange -= gameListener.OnGameStateChange;
	}

	public void OnTimerDone () {
		changeState (GameState.Lost);
	}

	public void OnProgressComplete () {
		changeState (GameState.Won);
	}

	protected abstract void onStateChange(GameState newState);

	protected abstract bool isValidTransition(GameState newState);

	protected void changeState(GameState newState) {
		if (newState == CurrentState)
			return;

		if (!isValidTransition (newState))
			return;

		CurrentState = newState;
		onStateChange (CurrentState);

		if (OnGameStateChange != null)
			OnGameStateChange (CurrentState);
	}

	void Update() {
		if (CurrentState != GameState.Active)
			return;

		UpdateGame ();
	}

	public abstract void UpdateGame();
}
