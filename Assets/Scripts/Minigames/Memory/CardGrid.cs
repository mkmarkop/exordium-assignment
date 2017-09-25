using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGrid : MonoBehaviour, IGameListener, ICardClickListener {

	public GameTaskBehaviour GameTask;

	public Sprite[] CardFaces;

	public GameObject CardPrefab;

	public int Columns { get; private set; }
	public int Rows { get; private set; }

	public float HorizontalSpacing { get; private set; }
	public float VerticalSpacing { get; private set; }

	private GameObject[,] _cardGrid;

	private CardBehaviour _firstCard;
	private CardBehaviour _secondCard;

	private GridState _currentState = GridState.Inactive;
	private const float PAIR_VISIBILITY_TIME = .75f;
	private float _nextInvisibleTime = 0f;

	private bool _gamePaused = true;

	void Start() {
		GameProxy.Instance.Register (this);
	}

	public void OnGameStateChange(GameState newState) {
		if (newState != GameState.Active) {
			_gamePaused = true;
		} else {
			_gamePaused = false;
		}
	}

	public void OnCardClick (CardBehaviour cardClicked) {
		switch (_currentState) {
		case GridState.Inactive:
			if (!cardClicked.Disabled) {
				_firstCard = cardClicked;
				_onStateChange (GridState.OneRevealed);
			}
			break;

		case GridState.OneRevealed:
			if (!cardClicked.Disabled) {
				_secondCard = cardClicked;
				_onStateChange (GridState.TwoRevealed);
			}
			break;

		case GridState.TwoRevealed:
			break;
		}
	}

	bool _isValidTransition(GridState newState) {
		bool isValid = false;

		switch (_currentState) {
		case GridState.Inactive:
			if (newState == GridState.OneRevealed)
				isValid = true;
			break;

		case GridState.OneRevealed:
			if (newState == GridState.TwoRevealed)
				isValid = true;
			break;

		case GridState.TwoRevealed:
			if (newState == GridState.Inactive)
				isValid = true;
			break;
		}

		return isValid;
	}

	void _onStateChange(GridState newState) {
		if (_currentState == newState)
			return;

		if (!_isValidTransition (newState))
			return;

		switch (newState) {
		case GridState.Inactive:
			if (_firstCard != null)
				_firstCard.Hide ();
			if (_secondCard != null)
				_secondCard.Hide ();
			break;

		case GridState.OneRevealed:
			_firstCard.Reveal ();
			GameTask.TakeStep ();
			break;

		case GridState.TwoRevealed:
			_secondCard.Reveal ();
			if (_firstCard.PairsWith (_secondCard)) {
				_firstCard.Validate ();
				_secondCard.Validate ();
				_firstCard = null;
				_secondCard = null;
				_nextInvisibleTime = Time.time;
				GameTask.TakeGoalStep ();
			} else {
				GameTask.TakeStep ();
				_secondCard.Invalidate ();
				_firstCard.Invalidate ();
				_nextInvisibleTime = Time.time + PAIR_VISIBILITY_TIME;
			}
			break;
		}

		_currentState = newState;
	}

	void Update() {
		switch (_currentState) {
		case GridState.Inactive:
			break;

		case GridState.OneRevealed:
			break;

		case GridState.TwoRevealed:
			if (_gamePaused) {
				_nextInvisibleTime += Time.deltaTime;
			} else {
				if (Time.time > _nextInvisibleTime) {
					_nextInvisibleTime = 0f;
					_onStateChange (GridState.Inactive);
				}
			}
			break;
		}
	}

	public void Clear() {
		foreach (Transform child in transform) {
			GameObject.Destroy (child.gameObject);
		}
	}

	void _initalize(int width, int height, float horizontalSpacing,
		float verticalSpacing) {
		Columns = width;
		Rows = height;
		HorizontalSpacing = horizontalSpacing;
		VerticalSpacing = verticalSpacing;
		_cardGrid = new GameObject[height, width];
		_firstCard = null;
		_secondCard = null;
		_currentState = GridState.Inactive;
		_nextInvisibleTime = 0f;
		_gamePaused = false;
	}

	Stack<T> _createShuffledDeck<T>(T[] items, int noPairs) {
		if (items.Length < noPairs)
			return null;

		var list = new List<T> ();
		IEnumerable<T> subItems = items.Take (noPairs);
		list.AddRange (subItems);
		list.AddRange (subItems);
		list.Shuffle ();

		return new Stack<T> (list);
	}

	public void PopulateGrid(int cols, int rows, float horizontalSpacing,
		float verticalSpacing) {
		_initalize (cols, rows, horizontalSpacing, verticalSpacing);
		Vector3 cardSize = new Vector3 (1.4f, 1.9f, 0);

		float width = Columns * cardSize.x + (Columns - 1) * HorizontalSpacing;
		float height = Rows * cardSize.y + (Rows - 1) * VerticalSpacing;
		Vector3 startPos = transform.position
			- new Vector3 (width / 2, -height / 2, 0);

		int numberOfPairs = (Columns * Rows) / 2;
		Stack<Sprite> shuffledDeck = _createShuffledDeck (CardFaces, numberOfPairs);

		for (int y = 0; y < Rows; y++) {
			for (int x = 0; x < Columns; x++) {
				GameObject card = (GameObject)Instantiate (CardPrefab);
				SpriteRenderer cardRenderer = card.GetComponent<SpriteRenderer> ();
				cardRenderer.sprite = shuffledDeck.Pop ();

				card.transform.parent = this.transform;
				float xPos = startPos.x + x * (cardSize.x + HorizontalSpacing);
				float yPos = startPos.y - y * (cardSize.y + VerticalSpacing);
				card.transform.position = new Vector3 (xPos, yPos,
					card.transform.position.z);
				_cardGrid [y, x] = card;
			}
		}
	}


}
