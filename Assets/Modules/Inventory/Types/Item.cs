using UnityEngine;

namespace Game {
	public abstract class Item : ScriptableObject {
		public new string name;
		public GameObject prefab;
	}
}