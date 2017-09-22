public delegate void ProgressHandler(int progress, int goal);
public delegate void ProgressCompletionHandler();

public interface IProgressPublisher {

	event ProgressHandler OnProgressStep;
	event ProgressCompletionHandler OnProgressComplete;

	void Register(IProgressListener progressListener);
	void Unregister(IProgressListener progressListener);

	void Register(IProgressCompletionListener completionListener);
	void Unregister(IProgressCompletionListener completionListener);
}
