using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour {

	public GameObject CardBack;

	public bool Disabled { get; private set; }

	public Sprite CardBackground;
	public Sprite CardValid;
	public Sprite CardInvalid;

	private SpriteRenderer _faceRenderer;
	private SpriteRenderer _backRenderer;

	// Use this for initialization
	void Start () {
		_faceRenderer = GetComponent<SpriteRenderer> ();
		_backRenderer = CardBack.GetComponent<SpriteRenderer> ();
		Hide ();
	}

	public void Reveal() {
		_backRenderer.enabled = false;
		Disabled = true;
	}

	public void Hide() {
		_backRenderer.sprite = CardBackground;
		_backRenderer.color = new Color (1f, 1f, 1f, 1f);
		_backRenderer.enabled = true;
		Disabled = false;
	}

	public void Validate() {
		_backRenderer.sprite = CardValid;
		_backRenderer.color = new Color (1f, 1f, 1f, .5f);
		_backRenderer.enabled = true;
		Disabled = true;
	}

	public void Invalidate() {
		_backRenderer.sprite = CardInvalid;
		_backRenderer.color = new Color (1f, 1f, 1f, .5f);
		_backRenderer.enabled = true;
		Disabled = true;
	}

	public bool PairsWith(CardBehaviour otherCard) {
		return _faceRenderer.sprite.name.Equals (
			otherCard._faceRenderer.sprite.name);
	}
}
