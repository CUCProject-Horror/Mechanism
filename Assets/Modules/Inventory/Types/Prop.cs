using UnityEngine;

namespace Game {
	[CreateAssetMenu(menuName = "Game/Item/Prop")]
	public class Prop : Item {
		public Prop() => type = ItemType.Prop;
	}
}