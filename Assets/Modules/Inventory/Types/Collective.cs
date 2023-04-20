using UnityEngine;

namespace Game {
	[CreateAssetMenu(menuName = "Game/Item/Collective")]
	public class Collective : Item {
		public Collective() => type = ItemType.Collective;
	}
}