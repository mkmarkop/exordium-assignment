using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTaskProxy : AbstractGameTask, IGameManagerListener {

	public AbstractGameTask TrueTask;

	private List<IProgressListener> _progressListeners = new List<IProgressListener> ();
	private List<IProgressCompletionListener> _completionListeners =
		new List<IProgressCompletionListener>();

	private bool _ingame = false;

	void Start() {
		GameProxy.Instance.Register ((IGameManagerListener)this);
	}

	public override void Initialize (int goalStepsRequired) {
		TrueTask.Initialize (goalStepsRequired);
	}

	public override void TakeStep () {
		TrueTask.TakeStep ();
	}

	public override void TakeGoalStep () {
		TrueTask.TakeGoalStep ();
	}

	public override void RevertGoalStep () {
		TrueTask.RevertGoalStep ();
	}

	public void OnGameLoad () {
		foreach (IProgressListener ipl in _progressListeners) {
			TrueTask.Register (ipl);
		}

		foreach (IProgressCompletionListener ipcl in _completionListeners) {
			TrueTask.Register (ipcl);
		}

		_ingame = true;
	}

	public void OnGameExit () {
		_ingame = false;

		foreach (IProgressListener ipl in _progressListeners) {
			TrueTask.Unregister (ipl);
		}

		foreach (IProgressCompletionListener ipcl in _completionListeners) {
			TrueTask.Unregister (ipcl);
		}
	}

	public override void Register (IProgressListener progressListener) {
		if (_ingame) {
			TrueTask.Register (progressListener);
		} else {
			_progressListeners.Add (progressListener);
		}
	}

	public override void Unregister (IProgressListener progressListener)	{
		if (_ingame) {
			TrueTask.Unregister (progressListener);
		} else {
			_progressListeners.Remove (progressListener);
		}
	}

	public override void Register (IProgressCompletionListener completionListener) {
		if (_ingame) {
			TrueTask.Register (completionListener);
		} else {
			_completionListeners.Add (completionListener);
		}
	}

	public override void Unregister (IProgressCompletionListener completionListener)	{
		if (_ingame) {
			TrueTask.Unregister (completionListener);
		} else {
			_completionListeners.Remove (completionListener);
		}
	}
}
