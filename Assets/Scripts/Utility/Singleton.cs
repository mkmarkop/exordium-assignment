using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	
	private static T _instance;

	private static object _lock = new object();

	public static T Instance {
		get {
			lock (_lock) {
				if (_instance == null) {
					_instance = (T)FindObjectOfType (typeof(T));
					if (_instance == null) {
						GameObject singleton = new GameObject ();
						_instance = singleton.AddComponent<T> ();
						DontDestroyOnLoad (_instance);
					}
				}
			}

			return _instance;
		}
	}
}
