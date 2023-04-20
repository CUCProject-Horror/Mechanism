using UnityEngine;

namespace Game {
	[CreateAssetMenu(menuName = "Game/Item/Treasure")]
	public class Treasure : Item {
		public Treasure() => type = ItemType.Treasure;
	}
}
