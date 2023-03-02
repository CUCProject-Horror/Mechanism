using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;

namespace Game {
	public class GameManager : MonoBehaviour {
		public static GameManager instance;
		public GameManager() => instance = this;

		public enum StateEnum {
			Invalid = 0,
			Protagonist = 1,
			Inventory
		}

		#region Inspector fields
		public float inventoryViewingDistance;
		public float inspectingDistance;
		#endregion

		#region Core fields
		[NonSerialized] public UIManager ui;
		[NonSerialized] public Protagonist protagonist;
		[NonSerialized] public DialogueSystemController dialogue;
		[NonSerialized] public InventoryUI inventoryUI;

		StateEnum state = StateEnum.Protagonist;
		#endregion

		#region Public interfaces
		public StateEnum State {
			get => state;
			set {
				switch(state = value) {
					case StateEnum.Invalid:
						state = StateEnum.Invalid;
						Cursor.lockState = CursorLockMode.None;
						protagonist.input.enabled = false;
						ui.Deactivate();
						break;
					case StateEnum.Protagonist:
						state = StateEnum.Protagonist;
						protagonist.input.enabled = true;
						Cursor.lockState = CursorLockMode.Locked;
						ui.SwitchTo(ui.aim);
						break;
					case StateEnum.Inventory:
						Cursor.lockState = CursorLockMode.None;
						protagonist.input.enabled = false;
						if(inventoryUI.currentCat != null)
							inventoryUI.SwitchCategoryTab(inventoryUI.currentCat);
						ui.SwitchTo(ui.inventory);
						break;
				}
			}
		}
		#endregion

		#region Life cycle
		void Start() {
			ui = GetComponent<UIManager>();
			protagonist = FindObjectOfType<Protagonist>(true);
			dialogue = GetComponent<DialogueSystemController>();
			inventoryUI = FindObjectOfType<InventoryUI>(true);

			State = StateEnum.Protagonist;
		}
		#endregion
	}
}