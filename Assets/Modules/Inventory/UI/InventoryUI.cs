using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Game {
	public class InventoryUi : UiBase {
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
			new Category(
				"Treasure",
				typeof(Treasure)
			),
		};

		[Serializable] public struct Prefabs {
			public GameObject categoryBtn;
			public GameObject itemBtn;
			public GameObject actionBtn;
			public GameObject itemUi;
		}
		public Prefabs prefabs;

		[Serializable] public struct Anchor {
			public Transform categories;
			public Transform items;
			public Transform actions;
		}
		public Anchor anchors;
		#endregion

		#region Inspector field
		public Transform anchor;
		#endregion

		#region Public interfaces
		public void SetItem(Item item) {
			anchor.DestroyAllChildren();
			anchor.rotation = Quaternion.identity;

			currentItem = item;
			if(item == null)
				return;

			var model = Instantiate(item.prefab, anchor);
			model.layer = LayerMask.NameToLayer("Inventory");
			var renderer = model.GetComponentInChildren<Renderer>();
			renderer.renderingLayerMask = 2;
		}

		public void Close() => GameManager.instance.State = GameManager.StateEnum.Protagonist;

		public void Inspect() {
			GameObject itemUiObj = Instantiate(prefabs.itemUi, transform);
			ItemUi itemUi = itemUiObj.GetComponent<ItemUi>();
			itemUi.Item = Item;
			ForwardTo(itemUi);
		}
		#endregion

		#region Gameplay
		Item currentItem;
		List<Item> items;
		public Category currentCat;

		public void UpdateItems(Category cat) {
			items = GameManager.instance.protagonist.inventory.items
				.Select(record => record.item)
				.Where(item => item.GetType() == cat.type)
				.ToList();
		}

		public void SwitchCategoryTab(Category cat) {
			currentCat = cat;
			UpdateItems(cat);
			anchors.items.DestroyAllChildren();
			if(items.Count == 0) {
				Item = null;
				return;
			}
			foreach(Item item in items) {
				GameObject itemBtn = Instantiate(prefabs.itemBtn, anchors.items);
				itemBtn.GetComponentInChildren<Text>().text = item.name;
                Button button = itemBtn.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => Item = item);
				button.onClick.AddListener(item.onView.Invoke);
			}
			if(Item?.GetType() != cat.type)
				Item = items[0];
		}

		public void UpdateButtons() {
			anchors.actions.DestroyAllChildren();
			var actions = new List<KeyValuePair<string, Action>> {
				new KeyValuePair<string, Action>("Close", Close),
				new KeyValuePair<string, Action>("Inspect", Inspect),
			};
			// Add more custom buttons
			foreach(var pair in actions) {
				GameObject btn = Instantiate(prefabs.actionBtn, anchors.actions);
				btn.GetComponentInChildren<Text>().text = pair.Key;
				btn.GetComponentInChildren<Button>().onClick.AddListener(pair.Value.Invoke);
			}
		}

		public Item Item {
			get => currentItem;
			set {
				SetItem(value);
				UpdateButtons();
			}
		}
		#endregion

		#region Life cycle
		void OnEnable() {
			anchors.items.DestroyAllChildren();
			anchors.actions.DestroyAllChildren();
			Item = currentItem;
		}

		void Start() {
			currentCat = categories[0];
			anchors.categories.DestroyAllChildren();
			foreach(Category tab in categories) {
				tab.element = Instantiate(prefabs.categoryBtn, anchors.categories);
				tab.text = tab.element.GetComponentInChildren<Text>();
				tab.text.text = tab.name;
				tab.button = tab.element.GetComponentInChildren<Button>();
				tab.button.onClick.AddListener(() => SwitchCategoryTab(tab));
			}
		}
		#endregion
	}
}
