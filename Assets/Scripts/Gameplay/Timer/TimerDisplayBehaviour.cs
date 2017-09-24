using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TimerDisplayBehaviour : MonoBehaviour, ITimerTickListener {

	private Text _textObj;
	public AbstractTimer Timer;

	// Use this for initialization
	void Start () {
		if (Timer != null) {
			Timer.Register (this);
		}

		_textObj = GetComponent<Text> ();
	}
	
	public void OnTimerTick (float seconds) {
		int minutePart = (int) seconds / 60;
		int secondPart = Mathf.CeilToInt (seconds % 60);
		_textObj.text = minutePart + ":" + secondPart;
	}
}
