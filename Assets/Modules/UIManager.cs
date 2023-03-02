using UnityEngine;
using System.Collections.Generic;

namespace Game {
	public class UIManager : MonoBehaviour {
		public RectTransform aim, inventory;
		public IEnumerable<RectTransform> all => new RectTransform[] { aim, inventory };

		public void Deactivate() {
			foreach(var ui in all)
				ui.gameObject.SetActive(false);
		}

		public void Activate(RectTransform ui) => ui.gameObject.SetActive(true);

		public void SwitchTo(RectTransform ui) {
			Deactivate();
			Activate(ui);
		}
	}
}