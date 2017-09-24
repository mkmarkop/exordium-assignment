using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerProxy : AbstractTimer, IGameManagerListener {

	public AbstractTimer TrueTimer;

	private List<ITimerTickListener> _tickListeners = new List<ITimerTickListener>();
	private List<ITimerDoneListener> _doneListeners = new List<ITimerDoneListener>();
	private bool _ingame = false;

	void Start() {
		GameProxy.Instance.Register ((IGameManagerListener)this);
	}

	public override void ResetTimer () {
		TrueTimer.ResetTimer ();
	}

	public override void Tick () {
		TrueTimer.Tick ();
	}

	public void OnGameLoad () {
		foreach (ITimerTickListener ittl in _tickListeners) {
			TrueTimer.Register (ittl);
		}

		foreach (ITimerDoneListener itdl in _doneListeners) {
			TrueTimer.Register (itdl);
		}

		_ingame = true;
	}

	public void OnGameExit () {
		_ingame = false;

		foreach (ITimerTickListener ittl in _tickListeners) {
			TrueTimer.Unregister (ittl);
		}

		foreach (ITimerDoneListener itdl in _doneListeners) {
			TrueTimer.Unregister (itdl);
		}
	}

	public override void Register (ITimerTickListener tickListener) {
		if (_ingame) {
			TrueTimer.Register (tickListener);
		} else {
			_tickListeners.Add (tickListener);
		}
	}

	public override void Unregister (ITimerTickListener tickListener) {
		if (_ingame) {
			TrueTimer.Unregister (tickListener);
		} else {
			_tickListeners.Remove (tickListener);
		}
	}

	public override void Register (ITimerDoneListener doneListener) {
		if (_ingame) {
			TrueTimer.Register (doneListener);
		} else {
			_doneListeners.Add (doneListener);
		}
	}

	public override void Unregister (ITimerDoneListener doneListener) {
		if (_ingame) {
			TrueTimer.Unregister (doneListener);
		} else {
			_doneListeners.Remove (doneListener);
		}
	}

	public override void OnGameStateChange (GameState newState) {
		if (_ingame)
			TrueTimer.OnGameStateChange (newState);
	}
}
