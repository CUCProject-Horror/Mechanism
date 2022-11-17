using UnityEngine;
using UnityEngine.UI;

namespace Game {
	public class Inspect : MonoBehaviour {
		#region Inspector field
		public Transform anchor;
		public Button closeButton;
		#endregion

		#region Public interfaces
		public float ViewingDistance {
			get => anchor.localPosition.z;
			set => anchor.localPosition = Vector3.forward * value;
		}

		public bool ShowCloseButton {
			set => closeButton.gameObject.SetActive(value);
		}

		public void SetItem(Item item) {
			anchor.DestroyAllChildren();
			anchor.rotation = Quaternion.identity;

			if(item == null)
				return;

			var model = Instantiate(item.prefab, anchor);
			model.layer = LayerMask.NameToLayer("Inventory");
			var renderer = model.GetComponentInChildren<Renderer>();
			renderer.renderingLayerMask = 2;
		}

		public void Close() => GameManager.instance.CloseUI();
		#endregion
	}
}