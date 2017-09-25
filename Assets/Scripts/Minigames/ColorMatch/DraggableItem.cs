using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DraggableItem : MonoBehaviour {
	
	Vector3 _scale;

	List<DraggableItemSlot> _touchingSlots = new List<DraggableItemSlot>();
	List<DraggableItemSource> _touchingSources = new List<DraggableItemSource> ();

	private ItemColor _color;
	private SpriteRenderer _spriteRenderer;

	public bool Slotted { get; private set; }
	public DraggableItemSlot OccupiedSlot { get; private set; }

	private Vector3 _initalPos;

	public bool UseLocalBounds = false;
	public BoxCollider2D LocalBounds;

	public bool _returnOnRelease = true;

	public bool _canBeSlotted = true;
	public bool _destroyIfNotSlotted = false;
	public bool _destroyIfSlotted = false;

	public bool _canBeUnslotted = true;
	public bool _destroyOnUnslot = false;
	public bool _returnOnUnslot = false;

	public ItemColor DragColor {
		get {
			return _color;
		}

		set {
			_spriteRenderer.color = ItemUtility.ItemColorToRGB (value);
			_color = value;
		}
	}

	// Use this for initialization
	void Awake () {
		_scale = transform.localScale;
		_spriteRenderer = GetComponent<SpriteRenderer> (); 
		Slotted = false;
	}

	void Start() {
		_initalPos = transform.position;
	}

	void OnTriggerEnter2D(Collider2D colliderEntered) {
		if (colliderEntered.tag == "Slot") {
			_touchingSlots.Add (colliderEntered.GetComponent<DraggableItemSlot> ());
		} else if (colliderEntered.tag == "Source") {
			_touchingSources.Add (colliderEntered.GetComponent<DraggableItemSource> ());
		}
	}

	void OnTriggerExit2D(Collider2D colliderExited) {
		if (colliderExited.tag == "Slot") {
			_touchingSlots.Remove (colliderExited.GetComponent<DraggableItemSlot> ());
		} else if (colliderExited.tag == "Source") {
			_touchingSources.Remove (colliderExited.GetComponent<DraggableItemSource> ());
		}
	}

	T nearestItem<T>(List<T> itemList) where T : MonoBehaviour {
		T closestItem = null;
		float minDist = Mathf.Infinity;

		foreach (T item in itemList) {
			float dist = Vector3.Distance (item.transform.position,
				             transform.position);
			
			if (dist < minDist) {
				dist = minDist;
				closestItem = item;
			}
		}

		return closestItem;
	}

	public bool InsideLocalBounds(Bounds boundsToCheck) {
		if (!UseLocalBounds || LocalBounds == null)
			return true;

		var bounds = LocalBounds.bounds;
		return bounds.ContainsBounds (boundsToCheck);
	}

	public bool Grab() {
		if (Slotted) {
			if (_canBeUnslotted) {
				Unslot ();
				if (_destroyOnUnslot) {
					Destroy (this.gameObject);
					return false;
				} else if (_returnOnUnslot) {
					ReturnToInitialPos ();
					return false;
				}
			} else {
				return false;
			}
		}
		Vector3 newScale = _scale;
		newScale.Scale (new Vector3 (1.25f, 1.25f, 1f));
		transform.localScale = newScale;
		return true;
	}

	public bool Slot(DraggableItemSlot slot) {
		if (slot.Insert (this)) {
			if (!_destroyIfSlotted) {
				OccupiedSlot = slot;
				Slotted = true;
			} else {
				Destroy (this.gameObject);
				return false;
			}
		} else {
			Slotted = false;
		}

		return Slotted;
	}

	public void Unslot() {
		OccupiedSlot.Clear ();
		OccupiedSlot = null;
		Slotted = false;
	}

	private IEnumerator _waitForFixedUpdate() {
		yield return new WaitForFixedUpdate ();
		CheckAttachables ();
	}

	public void ReturnToInitialPos() {
		GetComponent<Rigidbody2D> ().MovePosition (_initalPos);
		StartCoroutine (_waitForFixedUpdate ());
	}

	public void OnEmptyRelease() {
		if (_returnOnRelease) {
			ReturnToInitialPos ();
		}
	}

	public bool CheckAttachables() {
		if (_touchingSlots.Count > 0) {
			if (!_canBeSlotted)
				return false;
			DraggableItemSlot slot = nearestItem<DraggableItemSlot> (_touchingSlots);
			return Slot (slot);
		} else if (_touchingSources.Count > 0) {
			DraggableItemSource sauce = nearestItem<DraggableItemSource> (_touchingSources);
			sauce.PutItem ();
			Destroy (this.gameObject);
			return true;
		}

		return false;
	}

	public void Release() {
		transform.localScale = _scale;

		if (!CheckAttachables()) {
			if (_destroyIfNotSlotted) {
				Destroy (this.gameObject);
			} else if (_returnOnRelease) {
				ReturnToInitialPos ();
			}
		}
	}
}
