using UnityEngine;
using UnityEngine.UI;

namespace Game {
	public class UiBase : MonoBehaviour {
		public virtual void Activate() {
			gameObject.SetActive(true);
			GetComponentInChildren<Selectable>()?.Select();
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