using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardGrid))]
public class Memory : AbstractGame, ICardClickPublisher {

	public LayerMask InteractiveLayer;

	public const int MINIMUM_A = 3;
	public const int MAXIMUM_A = 4;

	public const int MINIMUM_B = 2;
	public const int MAXIMUM_B = 5;

	public event CardClickHandler OnCardClick;

	public void Register (ICardClickListener clickListener) {
		OnCardClick += clickListener.OnCardClick;
	}

	public void Unregister (ICardClickListener clickListener) {
		OnCardClick -= clickListener.OnCardClick;
	}

	public override void InitializeGame () {
		Register (ScreenManager.Instance);
		int A = Random.Range (MINIMUM_A, MAXIMUM_A + 1);
		int B;

		if (A % 2 != 0) {
			B = 2 * Random.Range (MINIMUM_B / 2, MAXIMUM_B / 2 + 1);
			if (B < MINIMUM_B)
				B += 2;
		} else {
			B = Random.Range (MINIMUM_B, MAXIMUM_B + 1);
		}

		int width = Mathf.Max (A, B);
		int height = Mathf.Min (A, B);
		int noPairs = (width * height) / 2;
		gameTask.Initialize (noPairs);
		gameTimer.TimerLength = noPairs * 10f;
		GetComponent<CardGrid> ().PopulateGrid (width, height, 0.1f, 0.25f);
		GetComponent<CardGrid> ().GameTask = gameTask;
		Register ((ICardClickListener)GetComponent<CardGrid> ());
	}

	protected override void onStateChange (GameState newState) {
		switch (newState) {
		case GameState.Active:
			break;

		case GameState.Inactive:
			Unregister (ScreenManager.Instance);
			Unregister ((ICardClickListener)GetComponent<CardGrid> ());
			GetComponent<CardGrid> ().Clear ();
			break;

		case GameState.Lost:
			break;

		case GameState.Won:
			break;

		case GameState.Paused:
			break;
		}
	}

	protected override bool isValidTransition (GameState newState) {
		return true;
	}

	public override void UpdateGame () {
		if (_mouseClicked) {
			CardBehaviour cardClicked = _objectClicked ();
			if (cardClicked && OnCardClick != null) {
				OnCardClick (cardClicked);
			}
		}
	}

	public override int CalculateScore () {
		int needed = gameTask.GoalStepsRequired * 2;
		int cardsRevealed = gameTask.StepsTaken;
		int score = 0;

		if (cardsRevealed > 0 && cardsRevealed <= needed + 6) {
			score = 3;
		} else if (cardsRevealed > needed + 6 && cardsRevealed <= needed + 12) {
			score = 2;
		} else if (cardsRevealed > needed + 12 && cardsRevealed <= needed + 18) {
			score = 1;
		} else if (cardsRevealed > needed + 18) {
			score = 0;
		}

		return score;
	}

	CardBehaviour _objectClicked() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction,
			Mathf.Infinity, InteractiveLayer);
		if (hit.collider != null) {
			CardBehaviour cardBhvr = hit.collider.GetComponent<CardBehaviour> ();
			return cardBhvr;
		} else {
			return null;
		}
	}

	bool _mouseClicked {
		get {
			return Input.GetMouseButton (0);
		}
	}
}
