using UnityEngine;

public static class BoundsExtension {

	public static bool ContainsBounds(this Bounds container, Bounds target) {
		return container.Contains(target.min) && container.Contains(target.max);
	}
}
