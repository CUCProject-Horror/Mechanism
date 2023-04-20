using UnityEngine;

namespace Game {
	[CreateAssetMenu(menuName = "Game/Item/CD")]
	public class CD : Item {
		public CD() => type = ItemType.CD;
	}
}