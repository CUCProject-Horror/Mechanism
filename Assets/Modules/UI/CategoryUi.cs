using UnityEngine;
using System.Collections.Generic;

namespace Game.Ui {
	public class CategoryUi : UiController {
		ItemType category;
		List<ItemRecord> records;
		List<UiElement> entries;

		public string emptyText = "£¿£¿£¿";

		public ItemType Category {
			get => category;
			set {
				category = value;
				records = GameManager.instance.protagonist
					.inventory.GetRecordsByType(value);
				if(records == null)
					throw new UnityException($"Cannot find category \"${category}\" in protagonist inventory");
				SetUpHierarchy();
			}
		}

		void SetUpHierarchy() {
			Bp.backButton.navigation.down = null;
			Utils.DestroyAllChildren(Bp.entryList.transform);
			if(records == null)
				return;
			entries = new List<UiElement>();
			foreach(var record in records) {
				var entryObj = Instantiate(Bp.entryButtonPrefab, Bp.entryList.transform);
				var entry = entryObj.GetComponent<UiElement>();
				entries.Add(entry);
				if(!record.possessed) {
					entry.Text = emptyText;
					entry.Selectable = false;
				}
				else {
					entry.Text = record.item.name;
					// entry.onUse = TODO
				}
			}
			Bp.selectedElement = Bp.backButton;
			for(var i = 0; i < entries.Count; ++i) {
				var entry = entries[i];
				if(i == 0) {
					Bp.SelectedElement = entry;
					entry.navigation.up = Bp.backButton;
					Bp.backButton.navigation.down = entry;
				}
				if(i > 0)
					entry.navigation.up = entries[i - 1];
				if(i < entries.Count - 1)
					entry.navigation.down = entries[i + 1];
			}
		}

		protected void OnEnable() {
			Category = Category;
		}
	}
}