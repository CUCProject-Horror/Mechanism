using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Game {
	public class InventoryUI : MonoBehaviour {
		#region Gameplay irrelavant
		public class Category {
			public class Item {
				public readonly Game.Item item;

				public GameObject element;
				public Button button;

				public Item(Game.Item item, Category category) {
					this.item = item;
				}
			}

			public readonly string name;
			public readonly Type type;
			public List<Item> items = new List<Item>();
			public int lastIndex = -1;

			public GameObject element;
			public Button button;
			public Text text;

			public Category(string name, Type type) {
				this.name = name;
				this.type = type;
			}

			public void UpdateItems() {
				items.Clear();
				items.AddRange(GameManager.instance.protagonist.inventory.items
					.Select(record => record.item)
					.Where(item => item.GetType() == type)
					.Select(item => new Item(item, this))
				);
			}
		}
		Category[] categories = new Category[] {
			new Category(
				"Collective",
				typeof(Collective)
			),
			new Category(
				"Prop",
				typeof(Prop)
			),
			new Category(
				"CD",
				typeof(CD)
			),
		};

		[Serializable] public struct Prefabs {
			public GameObject categoryBtn;
			public GameObject itemBtn;
			public GameObject actionBtn;
		}
		public Prefabs prefabs;

		[Serializable] public struct Pivots {
			public Transform categories;
			public Transform items;
			public Transform actions;
		}
		public Pivots pivots;

		void Start() {
			pivots.categories.DestroyAllChildren();
			foreach(Category tab in categories) {
				tab.element = Instantiate(prefabs.categoryBtn, pivots.categories);
				tab.text = tab.element.GetComponentInChildren<Text>();
				tab.text.text = tab.name;
				tab.button = tab.element.GetComponentInChildren<Button>();
				tab.button.onClick.AddListener(() => SwitchCategoryTab(tab));
			}
		}
		#endregion

		#region Gameplay
		Item currentItem;
		GameObject currentModel;
		List<Item> items;

		public void UpdateItems(Category cat) {
			items = GameManager.instance.protagonist.inventory.items
				.Select(record => record.item)
				.Where(item => item.GetType() == cat.type)
				.ToList();
		}

		public void SwitchCategoryTab(Category cat) {
			UpdateItems(cat);
			pivots.items.DestroyAllChildren();
			if(items.Count == 0) {
				Item = null;
				return;
			}
			foreach(Item item in items) {
				GameObject itemBtn = Instantiate(prefabs.itemBtn, pivots.items);
				itemBtn.GetComponentInChildren<Text>().text = item.name;
				itemBtn.GetComponentInChildren<Button>().onClick.AddListener(() => Item = item);
			}
			if(Item?.GetType() != cat.type)
				Item = items[0];
		}

		public void UpdateButtons() {
			pivots.actions.DestroyAllChildren();
			var actions = new List<KeyValuePair<string, Action>> {
				new KeyValuePair<string, Action>("Close", () => GameManager.instance.CloseUI())
			};
			foreach(var pair in actions) {
				GameObject btn = Instantiate(prefabs.actionBtn, pivots.actions);
				btn.GetComponentInChildren<Text>().text = pair.Key;
				btn.GetComponentInChildren<Button>().onClick.AddListener(pair.Value.Invoke);
			}
		}

		public void ViewItem(Item item) {
			if(currentModel) {
				Destroy(currentModel);
				currentModel = null;
			}
			if(currentItem = item) {
				currentModel = Instantiate(item.prefab, transform);
				currentModel.layer = LayerMask.NameToLayer("Inventory");
				var renderer = currentModel.GetComponentInChildren<Renderer>();
				renderer.renderingLayerMask = 2;
				Category cat = categories.First(cat => cat.type == item.GetType());
				SwitchCategoryTab(cat);
			}
			UpdateButtons();
		}

		public Item Item {
			get => currentItem;
			set {
				if(value != Item)
					ViewItem(value);
			}
		}

		void OnEnable() {
			pivots.items.DestroyAllChildren();
			pivots.actions.DestroyAllChildren();

			ViewItem(Item);
		}
		#endregion
	}
}
