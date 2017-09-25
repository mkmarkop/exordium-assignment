using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggableItemSource : MonoBehaviour {

	private const char INFINITY_SIGN = '\u221E';

	public Text ItemCounter;

	public SpriteRenderer ColorIcon;

	public DraggableItem ItemPrefab;

	public ItemColor SpawnColor;

	public int ItemsInitially { get; private set; }

	public BoxCollider2D ImposedBounds = null;

	private int _itemsInside = 1;

	public void Initialize(int items) {
		ItemsInitially = items;
		_itemsInside = items;
	}

	public bool IsDepleted {
		get {
			return _itemsInside == 0;
		}
	}

	public void PutItem() {
		if (IsDepleted) {
			Color temp = ColorIcon.color;
			temp.a = 1f;
			ColorIcon.color = temp;
		}
		if (_itemsInside > -1) {
			_itemsInside++;
			UpdateText ();
		}
	}

	public DraggableItem TakeItem() {
		if (IsDepleted)
			return null;

		if (_itemsInside > 0)
			_itemsInside--;

		if (IsDepleted) {
			Color temp = ColorIcon.color;
			temp.a = 0.65f;
			ColorIcon.color = temp;
		}

		UpdateText ();

		DraggableItem newItem = (DraggableItem)GameObject.Instantiate (ItemPrefab);
		newItem.DragColor = SpawnColor;
		newItem.transform.position = transform.position;
		newItem.transform.SetParent (GameProxy.Instance.CurrentGame.transform);
		if (ImposedBounds != null) {
			newItem.UseLocalBounds = true;
			newItem.LocalBounds = ImposedBounds;
		}

		return newItem;
	}

	public void UpdateText() {
		string text = "" + INFINITY_SIGN;
		if (_itemsInside > -1) {
			text = _itemsInside.ToString ();
		}

		ItemCounter.text = text;
	}

	void Start() {
		ColorIcon.color = ItemUtility.ItemColorToRGB (SpawnColor);
		UpdateText ();
	}
}
