using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Game {
	public class InventoryUI : MonoBehaviour {
		#region Static
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
			public TMP_Text text;

			public Category(string name, Type type) {
				this.name = name;
				this.type = type;
			}

			public void UpdateItems() {
				items.Clear();
				items.AddRange(Protagonist.instance.inventory.items
					.Select(record => record.item)
					.Where(item => item.GetType() == type)
					.Select(item => new Item(item, this))
				);
				//
			}
		}
		Category[] categories;

		public static InventoryUI instance;
		public InventoryUI() {
			instance = this;
			categories = new Category[]{
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
		}

		struct Prefabs {
			public GameObject categoryBtn;
			public GameObject itemBtn;
			public GameObject actionBtn;
		}
		Prefabs prefabs;

		void Awake() {
			prefabs.categoryBtn = Resources.Load<GameObject>("Category Button");
			prefabs.itemBtn = Resources.Load<GameObject>("Item Button");
			prefabs.actionBtn = Resources.Load<GameObject>("Action Button");
		}
		#endregion

		#region Gameplay irrelavant
		[Serializable]
		public struct Pivots {
			public Transform categories;
			public Transform items;
			public Transform actions;
		}
		public Pivots pivots;

		void Start() {
			pivots.categories.DestroyAllChildren();
			foreach(Category tab in categories) {
				tab.element = Instantiate(prefabs.categoryBtn, pivots.categories);
				tab.text = tab.element.GetComponentInChildren<TMP_Text>();
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
			items = Protagonist.instance.inventory.items
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
				itemBtn.GetComponentInChildren<TMP_Text>().text = item.name;
				itemBtn.GetComponentInChildren<Button>().onClick.AddListener(() => Item = item);
			}
			if(Item?.GetType() != cat.type)
				Item = items[0];
		}

		public void UpdateButtons() {
			pivots.actions.DestroyAllChildren();
			var actions = new List<KeyValuePair<string, Action>>();
			actions.Add(new KeyValuePair<string, Action>("Close", Close));
			foreach(var pair in actions) {
				GameObject btn = Instantiate(prefabs.actionBtn, pivots.actions);
				btn.GetComponentInChildren<TMP_Text>().text = pair.Key;
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

		public void Open() {
			gameObject.SetActive(true);
			Protagonist.instance.Input = false;
		}

		public void Close() {
			gameObject.SetActive(false);
			Protagonist.instance.Input = true;
		}

		void OnEnable() {
			Protagonist.instance.UI = true;

			pivots.items.DestroyAllChildren();
			pivots.actions.DestroyAllChildren();

			ViewItem(Item);
		}

		void OnDisable() {
			Protagonist.instance.Input = true;
		}
		#endregion
	}
}