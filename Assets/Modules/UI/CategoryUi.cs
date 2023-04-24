using UnityEngine;
using System.Collections.Generic;

namespace Game.Ui {
	public class CategoryUi : UiController {
		ItemType category;
		List<ItemRecord> records;

		public string emptyText = "������";

		public ItemType Category {
			get => category;
			set {
				category = value;
				records = GameManager.instance.protagonist
					.inventory.GetRecordsByType(value);
				if(records == null)
					throw new UnityException($"Cannot find category \"${category}\" in protagonist inventory");
				SetUpEntries();
			}
		}

		void CleanUpEntries() {
			Bp.backButton.navigation.down = null;
			Utils.DestroyAllChildren(Bp.entryList.transform);
		}

		void SetUpEntries() {
			CleanUpEntries();
			if(records == null)
				return;
			var entries = new List<UiElement>();
			foreach(var record in records) {
				var entryObj = Instantiate(Bp.entryButtonPrefab, Bp.entryList.transform);
				var entry = entryObj.GetComponent<UiElement>();
				entries.Add(entry);
				if(!record.possessed) {
					entry.Selectable = true;
					entry.Text = emptyText;
				}
				else {
					entry.Selectable = true;
					entry.Text = record.item.name;
				}
			}
			Bp.SelectedElement = Bp.backButton;
			for(int i = 0; i < entries.Count; ++i) {
				var entry = entries[i];
				if(i == 0) {
					Bp.SelectedElement = entry;
					Bp.backButton.navigation.down = entry;
					entry.navigation.up = Bp.backButton;
				}
				// TODO
			}
			Bp.SetUpEntriesNagivation();
		}

		void OnEntrySelect() {
			var entry = Bp.SelectedElement;
			// TODO
		}

		protected void Start() {
			// Could be set in inspector
			Bp.onEntrySelect.AddListener(() => {
				OnEntrySelect();
			});
		}

		protected void OnEnable() {
			SetUpEntries();
		}

		protected void OnDisable() {
			CleanUpEntries();
		}
	}
}