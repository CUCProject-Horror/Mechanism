using UnityEngine;

namespace Game.Ui {
	public class PauseUi : UiController {
		public void OpenInventory() {
			var ui = GameManager.instance.ui;
			ui.Open(ui.inventoryUi.Page);
		}
	}
}