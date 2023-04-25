using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Game {
	public enum ItemType {
		Collective, Prop, CD, Treasure,
	}

	public abstract class Item : ScriptableObject {
		public new string name;
		public ItemType type;
		public GameObject prefab;
		public Sprite selectSprite;
		public bool showDescription = true;
		[ShowIf("showDescription")]
		[ResizableTextArea]
		public string description;

		public UnityEvent onView;
	}
}