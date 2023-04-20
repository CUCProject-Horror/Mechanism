using UnityEngine;
using UnityEngine.Events;

namespace Game {
	public enum ItemType {
		Collective, Prop, CD, Treasure,
	}

	public abstract class Item : ScriptableObject {
		public new string name;
		public ItemType type;
		public GameObject prefab;
		public Texture selectTex;

		public UnityEvent onView;
	}
}