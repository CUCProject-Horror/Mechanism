using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Game {
	public class ItemSource : MonoBehaviour {
		#region Inspector fields
		public Item item;
		[HideInInspector]public GameManager game;
		public bool infinite = false;
		[HideIf("infinite")] public uint count = 1;
		[HideIf("infinite")] public bool destroyOnEmpty;
		public UnityEvent onDeliver;
		[HideIf("infinite")] public UnityEvent onEmpty;
		#endregion

		#region Public interfaces

		
		public void Deliver(Inventory inventory) {
			if(item == null) {
				Debug.LogWarning("Item to deliver is null");
				return;
			}
			inventory.Possess(item);
			if(!infinite) {
				--count;
				if(destroyOnEmpty && count == 0) {
					onEmpty.Invoke();
					Destroy(gameObject);
				}
			}			
			item?.onView?.Invoke();
			onDeliver.Invoke();
			SelectItemOnDeliver(item);
		}

		public void SelectItemOnDeliver(Item item)
		{
			if (item.type != ItemType.CD)
			{
				var category = game.ui.categoryUi;

				//打开CategoryUI，并将SelectedElement变成该Item对应的按钮
				game.OpenInventoryDirectly();
				category.Category = item.type;
				game.ui.Open(category.Bp);
				int i = category.GetRecordIndexByItem(item);
				category.Bp.SelectedElement = category.GetEntryButtonByRecordIndex(i);
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
			game = FindObjectOfType<GameManager>();
		}

		void OnDrawGizmos() {
			var prefab = item?.prefab!;
			if (!prefab)
				return;
			Mesh mesh = prefab.GetComponentInChildren<MeshFilter>()?.sharedMesh;
			if(mesh)
				Gizmos.DrawMesh(mesh, transform.position, transform.rotation, prefab.transform.localScale);
			else
				Gizmos.DrawCube(transform.position, Vector3.one);
		}
		#endregion
	}
}