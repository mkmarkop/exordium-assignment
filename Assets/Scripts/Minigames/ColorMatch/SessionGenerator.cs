using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionGenerator : MonoBehaviour {

	// This item color, in order, won't be able to reach
	// slot of the infinite item color.
	public const int BLOCKED = 3;
	public const int MINIMUM_SLOTS = 4;
	public const int MAXIMUM_SLOTS = 8;
	public const int LIMITED_ITEMS = 3;

	public DraggableItemSource ItemSource;

	public DraggableItemSlot InfiniteSlotPrefab;

	public DraggableItemSlot LimitedSlotPrefab;

	public List<DraggableItemSlot> RandomSlotPrefabs;

	private List<DraggableItemSlot> _currentSlots = new List<DraggableItemSlot> ();
	private List<DraggableItemSource> _currentSources = new List<DraggableItemSource> ();

	private GameTaskBehaviour _gameTask;
	private BoxCollider2D _localBoundCollider = null;

	public void CreateBoard() {
		_createSlots ();
		_createSources ();
	}

	public int TotalSlots() {
		return _currentSlots.Count;
	}

	DraggableItemSlot _createSlot(DraggableItemSlot prefab, Transform parent,
		Vector3 position) {
		var slot = (DraggableItemSlot)Instantiate (prefab, parent);
		slot.transform.position = position;
		slot.GameTask = _gameTask;
		return slot;
	}

	void _createSlots() {
		GameObject slotHolder = new GameObject ("Slot Holder");
		slotHolder.transform.parent = transform;
		slotHolder.transform.position = Vector3.zero;

		int slots = Random.Range (MINIMUM_SLOTS, MAXIMUM_SLOTS + 1);

		float length = slots * 1f;
		Vector3 offset = new Vector3 (-length / 2f, 0f, 0f);

		var infiniteSlot = _createSlot (InfiniteSlotPrefab, slotHolder.transform, offset);
		offset += new Vector3 (1f, 0f, 0f);
		_currentSlots.Add (infiniteSlot);

		var limitedSlot = _createSlot (LimitedSlotPrefab, slotHolder.transform, offset);
		offset += new Vector3 (1f, 0f, 0f);
		_currentSlots.Add (limitedSlot);

		RandomSlotPrefabs.Shuffle ();
		var stack = new Stack<DraggableItemSlot> (RandomSlotPrefabs);

		for (int i = MINIMUM_SLOTS - 1; i <= slots; i++) {
			var slot = _createSlot (stack.Pop (), slotHolder.transform, offset);
			offset += new Vector3 (1f, 0f, 0f);
			_currentSlots.Add (slot);
		}

	}

	DraggableItemSource _createSource(Transform parent, Vector3 position,
		int items, ItemColor spawnColor) {
		var source = (DraggableItemSource)Instantiate (ItemSource, parent);
		source.transform.position = position;
		source.SpawnColor = spawnColor;
		source.Initialize (items);
		return source;
	}

	Bounds _screenBounds() {
		Bounds screenBounds = new Bounds ();
		screenBounds.max = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width, Screen.height));
		screenBounds.min = Camera.main.ScreenToWorldPoint (Vector3.zero);
		screenBounds.center = Vector3.zero;

		return screenBounds;
	}

	BoxCollider2D _avoidSlotBounds(DraggableItemSlot slot) {
		if (!_localBoundCollider) {
			_localBoundCollider = gameObject.AddComponent<BoxCollider2D> ();
			_localBoundCollider.isTrigger = true;
		}

		var bounds = slot.GetComponent<BoxCollider2D> ().bounds;
		float startX = bounds.max.x;

		var screenBounds = _screenBounds ();

		var avoidBounds = new Bounds ();
		avoidBounds.max = screenBounds.max;
		avoidBounds.min = new Vector3(startX + 0.02f, screenBounds.min.y, screenBounds.min.z);
		_localBoundCollider.size = new Vector2 (avoidBounds.size.x, avoidBounds.size.y);
		_localBoundCollider.offset = avoidBounds.center;

		return _localBoundCollider;
	}

	void _createSources() {
		var itemsAvailable = new Dictionary<ItemColor, DraggableItemSource> ();

		GameObject sourceHolder = new GameObject ("Source Holder");
		sourceHolder.transform.parent = transform;
		sourceHolder.transform.position = new Vector3(0f, -2.25f, 0f);

		int sources = _currentSlots.Count - 1;
		float length = sources * 1f;
		Vector3 offset = _currentSlots [0].transform.TransformPoint (Vector3.zero)
		                 + new Vector3 (1f, -2.25f, 0f);

		var infiniteSource = _createSource (sourceHolder.transform, offset,
			                     -1, InfiniteSlotPrefab.DragColor);
		itemsAvailable.Add (infiniteSource.SpawnColor, infiniteSource);
		_currentSources.Add (infiniteSource);
		offset += new Vector3 (1f, 0f, 0f);

		var limitedSource = _createSource (sourceHolder.transform, offset,
			                  LIMITED_ITEMS, LimitedSlotPrefab.DragColor);
		itemsAvailable.Add (limitedSource.SpawnColor, limitedSource);
		_currentSources.Add (limitedSource);
		offset += new Vector3 (1f, 0f, 0f);

		for (int i = 0; i < _currentSlots.Count; i++) {
			var slot = _currentSlots [i];
			var colorsAccepted = slot.WhiteList;
			var colorsRejected = slot.BlackList;
			colorsAccepted.Shuffle ();

			bool found = false;

			foreach (ItemColor color in colorsAccepted) {
				if (itemsAvailable.ContainsKey (color)
					&& !colorsRejected.Contains(color)) {
					var currSource = itemsAvailable [color];
					if (currSource.ItemsInitially > -1)
						currSource.Initialize (currSource.ItemsInitially + 1);
					found = true;
					break;
				}
			}

			if (!found) {
				var source = _createSource (sourceHolder.transform, offset, 1,
					             colorsAccepted [Random.Range (0, colorsAccepted.Count)]);
				itemsAvailable.Add (source.SpawnColor, source);
				_currentSources.Add (source);
				offset += new Vector3 (1f, 0f, 0f);
			}
		}

		if (BLOCKED - 1 < _currentSources.Count)
			_currentSources [BLOCKED - 1].ImposedBounds = _avoidSlotBounds (_currentSlots [0]);
	}

	// Use this for initialization
	void Start () {
		_gameTask = GetComponent<GameTaskBehaviour> ();
	}

	public void ClearBoard() {
		_currentSlots = new List<DraggableItemSlot> ();
		_currentSources = new List<DraggableItemSource> ();
	}
}
