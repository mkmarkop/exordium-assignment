using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AudibleBehaviour : MonoBehaviour {

	private Button _button;

	// Use this for initialization
	void Start () {
		_button = GetComponent<Button> ();
		_button.onClick.AddListener (SoundManager.Instance.PlaySFX);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
