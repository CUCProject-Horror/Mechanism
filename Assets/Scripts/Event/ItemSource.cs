using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class ItemSource : MonoBehaviour {
	public Interactable interactor;
	public Item item;
	public bool infinite = false;
	[HideIf("infinite")] public uint count = 1;
	[HideIf("infinite")] public bool destroyOnEmpty;
	public UnityEvent onDeliver;
	[HideIf("infinite")] public UnityEvent onEmpty;

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

	public void Start() {
		if(interactor) {
			interactor.onInteract.AddListener((Component source) => {
				Inventory inventory = source?.GetComponent<Player>().inventory;
				if(inventory == null)
					return;
				Deliver(inventory);
			});
		}
	}
}
