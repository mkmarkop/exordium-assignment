public delegate void TimerTickHandler(float seconds);
public delegate void TimerDoneHandler();

public interface ITimerPublisher {

	void Register(ITimerTickListener tickListener);
	void Unregister(ITimerTickListener tickListener);

	void Register(ITimerDoneListener doneListener);
	void Unregister(ITimerDoneListener doneListener);
}
