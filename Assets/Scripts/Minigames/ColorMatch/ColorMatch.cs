using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SessionGenerator))]
public class ColorMatch : AbstractGame {
	
	public override void InitializeGame () {
		Register (ScreenManager.Instance);
		var sessionGen = GetComponent<SessionGenerator> ();
		sessionGen.ClearBoard ();
		sessionGen.CreateBoard ();
		_gameTask.Initialize (sessionGen.TotalSlots ());
		_gameTimer.TimerLength = sessionGen.TotalSlots () * 8f;
	}

	protected override void _onStateChange (GameState newState) {
		switch (newState) {
		case GameState.Active:
			break;

		case GameState.Inactive:
			Unregister (ScreenManager.Instance);
			GetComponent<SessionGenerator> ().ClearBoard ();
			foreach (Transform child in transform) {
				Destroy (child.gameObject);
			}
			break;

		case GameState.Lost:
			break;

		case GameState.Won:
			break;

		case GameState.Paused:
			break;
		}
	}

	protected override bool _isValidTransition (GameState newState) {
		return true;
	}

	public override void UpdateGame () {
		//
	}

	public override int CalculateScore () {
		int score = 0;
		int stepsTaken = _gameTask.StepsTaken;
		int stepsNeeded = _gameTask.GoalStepsRequired;

		if (stepsTaken <= stepsNeeded + 9
			&& stepsTaken >= stepsNeeded + 7) {
			score = 1;
		} else if (stepsTaken < stepsNeeded + 7
			&& stepsTaken >= stepsNeeded + 4) {
			score = 2;
		} else if (stepsTaken < stepsNeeded + 4) {
			score = 3;
		}

		return score;
	}
}
