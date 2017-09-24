using UnityEngine;

public class GameTaskBehaviour : AbstractGameTask {

	public event ProgressHandler OnProgressStep;

	public event ProgressCompletionHandler OnProgressComplete;

	public int StepsTaken { get; private set; }

	public int GoalStepsRequired { get; private set; }

	public int GoalStepsTaken { get; private set; }

	public override void Initialize(int goalStepsRequired) {
		GoalStepsRequired = goalStepsRequired;
		GoalStepsTaken = 0;
		StepsTaken = -1;
		TakeStep ();
	}

	public override void TakeStep() {
		StepsTaken++;
		if (OnProgressStep != null)
			OnProgressStep (GoalStepsTaken, GoalStepsRequired);
	}

	public override void TakeGoalStep() {
		GoalStepsTaken++;
		TakeStep();

		if (GoalStepsTaken >= GoalStepsRequired &&
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
