using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ProgressDisplayBehaviour : MonoBehaviour, IProgressListener {

	private Text _textObj;
	public AbstractGameTask GameTask;

	// Use this for initialization
	void Start () {
		if (GameTask != null) {
			GameTask.Register (this);
		}

		_textObj = GetComponent<Text> ();
	}

	public void OnProgressStep (int progress, int goal) {
		_textObj.text = progress + "/" + goal;
	}
}
