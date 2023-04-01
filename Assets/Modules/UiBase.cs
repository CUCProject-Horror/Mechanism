using UnityEngine;

namespace Game {
	public class UiBase : MonoBehaviour {
		public virtual void Activate() {
			gameObject.SetActive(true);
		}
		public virtual void Deactivate() {
			gameObject.SetActive(false);
		}

		public void ForwardTo(UiBase ui) {
			GameManager.instance.ui.ForwardTo(ui);
		}
		public void Back() {
			GameManager.instance.ui.Back();
		}
	}
}