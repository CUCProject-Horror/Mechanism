using UnityEngine;

namespace Game.Ui {
	public class InventoryUi : UiController {
		public void ViewCategory(ItemType type) {
			var ui = GameManager.instance.ui;
			CategoryUi cat = ui.categoryUi;
			cat.Category = type;
			ui.Open(cat.Page);
		}
		public void ViewCategory(string typeName) {
			var type = Utils.StringToEnum<ItemType>(typeName);
			ViewCategory(type);
		}
	}
}