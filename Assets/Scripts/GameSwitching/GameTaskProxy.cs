using UnityEngine;

public class GameTaskProxy : AbstractGameTask {

	public AbstractGameTask TrueTask;

	public override void Initialize (int goalStepsRequired) {
		TrueTask.Initialize (goalStepsRequired);
	}

	public override void TakeStep () {
		TrueTask.TakeStep ();
	}

	public override void TakeGoalStep () {
		TrueTask.TakeGoalStep ();
	}

	public override void Register (IProgressListener progressListener) {
		TrueTask.Register (progressListener);
	}

	public override void Unregister (IProgressListener progressListener)	{
		TrueTask.Unregister (progressListener);
	}

	public override void Register (IProgressCompletionListener completionListener) {
		TrueTask.Register (completionListener);
	}

	public override void Unregister (IProgressCompletionListener completionListener)	{
		TrueTask.Unregister (completionListener);
	}
}
