using UnityEngine;

public static class TransformExtension {

	public static void SnapTo(this Transform item, Transform slot) {
		item.transform.position = slot.position;
	}
}
