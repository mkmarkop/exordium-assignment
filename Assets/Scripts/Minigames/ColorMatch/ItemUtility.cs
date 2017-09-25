using UnityEngine;

public static class ItemUtility {

	public static Color ItemColorToRGB(ItemColor itemColor) {
		Color color = Color.red;

		switch (itemColor) {
		case ItemColor.Black:
			color = new Color(0.05f, 0.05f, 0.05f, 1f);
			break;
		case ItemColor.Blue:
			color = Color.blue;
			break;
		case ItemColor.Green:
			color = Color.green;
			break;
		case ItemColor.Orange:
			color = new Color(1f, 0.65f, 0, 1f);
			break;
		case ItemColor.Purple:
			color = new Color(0.5f, 0, 0.5f, 1f);
			break;
		case ItemColor.Red:
			color = Color.red;
			break;
		case ItemColor.White:
			color = Color.white;
			break;
		case ItemColor.Yellow:
			color = Color.yellow;
			break;
		}

		return color;
	}
}
