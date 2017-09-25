using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class DraggableItemSlot : MonoBehaviour {

	private ItemColor _color;
	private SpriteRenderer _spriteRenderer;

	public GameTaskBehaviour GameTask;

	public bool SnapItems;
	public Transform SnapPosition;
	public List<ItemColor> WhiteList;
	public List<ItemColor> BlackList;

	public bool Occupied { get; private set; }

	public ItemColor DragColor;
		
	// Use this for initialization
	void Awake () {
		_spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public bool Fits(DraggableItem item) {
		return WhiteList.Contains (item.DragColor)
		&& !BlackList.Contains (item.DragColor);
	}

	public bool Insert(DraggableItem item) {
		if (!SnapItems)
			return false;
		
		if (Fits (item) && !Occupied) {
			item.transform.SnapTo (SnapPosition);
			Occupied = true;
			GameTask.TakeGoalStep ();
			return true;
		} else {
			return false;
		}
	}

	public void Clear() {
		Occupied = false;
		GameTask.RevertGoalStep ();
	}
}
