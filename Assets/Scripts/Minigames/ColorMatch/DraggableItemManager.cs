using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableItemManager : MonoBehaviour, IGameListener {

	public LayerMask InteractiveLayer;
	public BoxCollider2D GlobalBounds;

	public bool IsDragging { get; private set; }
	public Vector3 Offset { get; private set; }

	public DraggableItem CurrentlyDragging;

	private GameState _currentGameState = GameState.Active;

	void Start () {
		IsDragging = false;
		GameProxy.Instance.Register (this);
	}

	void _grabItem(DraggableItem item) {
		if (item.Grab ()) {
			CurrentlyDragging = item;
			IsDragging = true;
			Offset = CurrentlyDragging.transform.position - _mousePosition ();
		}
	}

	Collider2D _objectClicked() {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction,
			Mathf.Infinity, InteractiveLayer);
		if (hit.collider != null) {
			return hit.collider;
		} else {
			return null;
		}
	}

	Vector3 _mousePosition() {
		Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return new Vector3 (mPos.x, mPos.y, 0f);
	}

	bool _mouseClicked {
		get {
			return Input.GetMouseButton (0);
		}
	}

	void _processClick(Collider2D objectClicked) {
		if (objectClicked == null)
			return;
		
		DraggableItemSource sourceComp = objectClicked.GetComponent<DraggableItemSource> ();
		if (sourceComp != null) {
			if (!sourceComp.IsDepleted)
				_grabItem (sourceComp.TakeItem ());
		} else {
			DraggableItem itemComp = objectClicked.GetComponent<DraggableItem> ();
			if (itemComp != null)
				_grabItem (itemComp);
		}
	}

	void _followMouse() {
		Bounds globalBounds = GlobalBounds.bounds;
		Bounds itemBounds = CurrentlyDragging.GetComponent<BoxCollider2D> ().bounds;
		Vector3 newPos = _mousePosition () + Offset;
		itemBounds.center = newPos;

		if (globalBounds.ContainsBounds (itemBounds) &&
			CurrentlyDragging.InsideLocalBounds(itemBounds)) {
			CurrentlyDragging.transform.position = newPos;
		}
	}

	void Update() {
		if (_currentGameState != GameState.Active)
			return;

		if (_mouseClicked) {
			if (!IsDragging) {
				_processClick (_objectClicked ());
			} else {
				_followMouse ();
			}
		} else {
			if (IsDragging) {
				_dropItem ();
			}
		}
	}

	void _dropItem() {
		IsDragging = false;
		CurrentlyDragging.Release ();
		CurrentlyDragging = null;
	}

	public void OnGameStateChange (GameState newState) {
		if (newState == GameState.Inactive) {
			IsDragging = false;
			CurrentlyDragging = null;
		}

		_currentGameState = newState;
	}
}
