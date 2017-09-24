using UnityEngine;

public abstract class AbstractGameTask : MonoBehaviour,
IProgressPublisher, IGameTask {

	public abstract void Initialize(int goalStepsRequired);

	public abstract void TakeStep();

	public abstract void TakeGoalStep();

	public abstract void Register(IProgressListener progressListener);

	public abstract void Unregister(IProgressListener progressListener);

	public abstract void Register(IProgressCompletionListener completionListener);

	public abstract void Unregister(IProgressCompletionListener completionListener);
}
