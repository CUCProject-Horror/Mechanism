using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Game {
	[CreateAssetMenu(menuName = "Game/Item/CD")]
	public class CD : Item {
		public UnityEvent playVid;

		public CD() => type = ItemType.CD;
	}
}