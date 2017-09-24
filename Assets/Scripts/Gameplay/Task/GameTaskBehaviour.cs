using UnityEngine;

public class GameTaskBehaviour : AbstractGameTask {

	public event ProgressHandler OnProgressStep;

	public event ProgressCompletionHandler OnProgressComplete;

	private int _stepsTaken;

	private int _goalStepsRequired;

	private int _goalStepsTaken;

	public override void Initialize(int goalStepsRequired) {
		_goalStepsRequired = goalStepsRequired;
		_goalStepsTaken = 0;
		_stepsTaken = -1;
		TakeStep ();
	}

	public override void TakeStep() {
		_stepsTaken++;
		if (OnProgressStep != null)
			OnProgressStep (_goalStepsTaken, _goalStepsRequired);
	}

	public override void TakeGoalStep() {
		_goalStepsTaken++;
		TakeStep();

		if (_goalStepsTaken >= _goalStepsRequired &&
			OnProgressComplete != null)
			OnProgressComplete ();
	}

	public override void Register (IProgressListener progressListener) {
		OnProgressStep += progressListener.OnProgressStep;
	}

	public override void Unregister (IProgressListener progressListener) {
		OnProgressStep -= progressListener.OnProgressStep;
	}

	public override void Register (IProgressCompletionListener completionListener) {
		OnProgressComplete += completionListener.OnProgressComplete;
	}

	public override void Unregister (IProgressCompletionListener completionListener) {
		OnProgressComplete -= completionListener.OnProgressComplete;
	}
}
