using UnityEngine;

public abstract class AbstractTimer : MonoBehaviour, ITimer,
ITimerPublisher, IGameListener {

	public abstract void ResetTimer ();

	public abstract void Tick ();

	public abstract void Register (ITimerTickListener tickListener);

	public abstract void Unregister (ITimerTickListener tickListener);

	public abstract void Register (ITimerDoneListener doneListener);

	public abstract void Unregister (ITimerDoneListener doneListener);

	public abstract void OnGameStateChange (GameState newState);
}
