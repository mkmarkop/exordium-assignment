using UnityEngine;

public class GameTaskBehaviour : MonoBehaviour, IProgressPublisher {

	public event ProgressHandler OnProgressStep;

	public event ProgressCompletionHandler OnProgressComplete;

	private int _stepsTaken;

	private int _goalStepsRequired;

	private int _goalStepsTaken;

	public void Initialize(int goalStepsRequired) {
		_goalStepsRequired = goalStepsRequired;
		_goalStepsTaken = 0;
		_stepsTaken = 0;
	}

	public void TakeStep() {
		_stepsTaken++;
		if (OnProgressStep != null)
			OnProgressStep (_goalStepsTaken, _goalStepsRequired);
	}

	public void TakeGoalStep() {
		TakeStep();
		_goalStepsTaken++;

		if (_goalStepsTaken >= _goalStepsRequired &&
		    OnProgressComplete != null)
			OnProgressComplete ();
	}

	public void Register (IProgressListener progressListener) {
		OnProgressStep += progressListener.OnProgressStep;
	}

	public void Unregister (IProgressListener progressListener) {
		OnProgressStep -= progressListener.OnProgressStep;
	}

	public void Register (IProgressCompletionListener completionListener) {
		OnProgressComplete += completionListener.OnProgressComplete;
	}

	public void Unregister (IProgressCompletionListener completionListener) {
		OnProgressComplete -= completionListener.OnProgressComplete;
	}
}
