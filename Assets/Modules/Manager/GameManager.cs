using UnityEngine;
using System;

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
		[NonSerialized] public InventoryUI inventoryUI;
		[NonSerialized] public InputManager input;

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
						input.enabled = false;
						ui.Deactivate();
						break;
					case StateEnum.Protagonist:
						state = StateEnum.Protagonist;
						input.enabled = true;
						Cursor.lockState = CursorLockMode.Locked;
						ui.SwitchTo(ui.aim);
						break;
					case StateEnum.Inventory:
						Cursor.lockState = CursorLockMode.None;
						input.enabled = false;
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
			inventoryUI = FindObjectOfType<InventoryUI>(true);
			input = GetComponent<InputManager>();

			State = StateEnum.Protagonist;
		}
		#endregion
	}
}
