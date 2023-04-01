using UnityEngine;
using System.Collections.Generic;

namespace Game {
	public class UIManager : MonoBehaviour {
		public UiBase aim, inventory;
		public IEnumerable<UiBase> all => new UiBase[] { aim, inventory };

		public void Deactivate() {
			foreach(var ui in all)
				ui.Deactivate();
		}

		public void Activate(UiBase ui) => ui.Activate();

		public void SwitchTo(UiBase ui) {
			Deactivate();
			Activate(ui);
		}
	}
}