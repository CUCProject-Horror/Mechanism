using UnityEngine;
using UnityEngine.Events;

namespace Game {
	public abstract class Item : ScriptableObject {
		public new string name;
		public GameObject prefab;
		public Texture selectTex;

		public UnityEvent onView;

		public void InspectSelf() {
			GameManager.instance.InspectItem(this);
		}
	}
}