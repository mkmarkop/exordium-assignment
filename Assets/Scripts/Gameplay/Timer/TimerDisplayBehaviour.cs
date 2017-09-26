using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TimerDisplayBehaviour : MonoBehaviour, ITimerTickListener {

	private Text _textObj;
	public AbstractTimer Timer;

	public enum TimerFormat {
		MinutesAndSeconds,
		SecondsOnly
	}

	public TimerFormat DisplayFormat = TimerFormat.MinutesAndSeconds;

	// Use this for initialization
	void Start () {
		if (Timer != null) {
			Timer.Register (this);
		}

		_textObj = GetComponent<Text> ();
	}

	void Update() {
		if (!GameProxy.Instance.TimersEnabled) {
			if (_textObj.text != "")
				_textObj.text = "";
		}
	}
	
	public void OnTimerTick (float seconds) {
		int minutePart = (int) (seconds / 60);
		int secondPart = Mathf.CeilToInt (seconds % 60);
		if (secondPart == 60) {
			secondPart = 0;
			minutePart = 1;
		}

		if (DisplayFormat == TimerFormat.SecondsOnly) {
			_textObj.text = Mathf.CeilToInt (seconds).ToString () + "s";
		} else {
			_textObj.text = minutePart.ToString ("00")
			+ ":" + secondPart.ToString ("00");
		}
	}
}
