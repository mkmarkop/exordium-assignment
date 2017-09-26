using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenBehaviour : ScreenBehaviour {

	public Text Instructions;

	protected override void _onActivate() {
		Instructions.text = GameProxy.Instance.CurrentGame.GameInstructions;
	}

}
