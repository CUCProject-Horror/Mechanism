using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ItemSource : MonoBehaviour {
	public Interactable interactor;
	public Item item;
	public bool infinite = false;
	[HideIf("infinite")] public uint count = 1;
	[HideIf("infinite")] public bool destroyOnEmpty;
	public UnityEvent onDeliver;
	[HideIf("infinite")] public UnityEvent onEmpty;
	public bool view = true;

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
		if(view) {
			PickupView view = FindObjectOfType<PickupView>(true);
			if(view) {
				FindObjectOfType<Protagonist>().Input = false;
				view.View(item);
			}
		}
	}

	public void Start() {
		if(!Application.isPlaying)
			return;
		if(interactor) {
			interactor.onInteract.AddListener((Component source) => {
				Inventory inventory = source?.GetComponent<Player>().inventory;
				if(inventory == null)
					return;
				Deliver(inventory);
			});
		}
	}

	public void Update() {
		if(!Application.isPlaying) {
			Mesh mesh = item.mesh ?? GetComponent<MeshFilter>().sharedMesh;
			if(mesh == null)
				return;
			GetComponent<MeshFilter>().sharedMesh = mesh;
			if(item.material)
				GetComponent<MeshRenderer>().sharedMaterial = item.material;
			MeshCollider collider = GetComponent<MeshCollider>();
			if(collider)
				collider.sharedMesh = mesh;
			return;
		}
	}
}
