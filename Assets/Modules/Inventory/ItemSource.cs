using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Game {
	public class ItemSource : MonoBehaviour {
		#region Inspector fields
		public Item item;
		public bool infinite = false;
		[HideIf("infinite")] public uint count = 1;
		[HideIf("infinite")] public bool destroyOnEmpty;
		public UnityEvent onDeliver;
		[HideIf("infinite")] public UnityEvent onEmpty;
		public bool view = true;
		#endregion

		#region Public interfaces
		public void Deliver(Inventory inventory) {
			if(item == null) {
				Debug.LogWarning("Item to deliver is null");
				return;
			}
			inventory.Add(item);
			onDeliver.Invoke();
			if(!infinite) {
				--count;
				if(destroyOnEmpty && count == 0) {
					onEmpty.Invoke();
					Destroy(gameObject);
				}
			}
		}

		public void DeliverToProtagonist() {
			Inventory inventory = GameManager.instance.protagonist.inventory;
			if(inventory == null)
				return;
			Deliver(inventory);
		}
		#endregion

		#region Life cycle
		void Start() {
			Instantiate(item.prefab, transform);
		}

		void OnDrawGizmos() {
			Mesh mesh = item?.prefab?.GetComponentInChildren<MeshFilter>()?.sharedMesh;
			if(mesh)
				Gizmos.DrawMesh(mesh, transform.position);
			else
				Gizmos.DrawCube(transform.position, Vector3.one);
		}
		#endregion
	}
}