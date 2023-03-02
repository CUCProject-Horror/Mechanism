using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;

namespace Game {
	public class GameManager : MonoBehaviour {
		#region Singleton
		public static GameManager instance;
		public GameManager() => instance = this;
		#endregion

		#region Inspector fields
		public float inventoryViewingDistance;
		public float inspectingDistance;
		#endregion

		#region Core fields
		[NonSerialized] public UIManager ui;
		[NonSerialized] public Protagonist protagonist;
		[NonSerialized] public DialogueSystemController dialogue;
		[NonSerialized] public Inspect inspect;
		[NonSerialized] public InventoryUI inventoryUI;

		State controlState = State.Protagonist;
		#endregion

		#region Auxiliary
		IEnumerator _SwitchState(State state) {
			switch(state) {
				case State.Invalid:
					controlState = State.Invalid;
					Cursor.lockState = CursorLockMode.None;
					protagonist.input.enabled = false;
					ui.Deactivate();
					break;
				case State.Protagonist:
					yield return new WaitForEndOfFrame();
					controlState = State.Protagonist;
					protagonist.input.enabled = true;
					Cursor.lockState = CursorLockMode.Locked;
					ui.SwitchTo(ui.aim);
					break;
				case State.Inventory:
					Cursor.lockState = CursorLockMode.None;
					protagonist.input.enabled = false;
					if(inventoryUI.currentCat != null)
						inventoryUI.SwitchCategoryTab(inventoryUI.currentCat);
					ui.SwitchTo(ui.inventory);
					break;
			}
		}
		#endregion

		#region Public interfaces
		public enum State {
			Invalid = 0,
			Protagonist = 1,
			Inventory,
			Inspect
		}
		public void SwitchState(State state) => StartCoroutine(_SwitchState(state));

		public void CloseUI() => SwitchState(controlState);

		public void OpenInventory() => SwitchState(State.Inventory);

		public void InspectItem(Item item) {
			inventoryUI.SetItem(item);
			SwitchState(State.Inspect);
		}
		#endregion

		#region Life cycle
		void Start() {
			ui = GetComponent<UIManager>();
			protagonist = FindObjectOfType<Protagonist>(true);
			dialogue = GetComponent<DialogueSystemController>();
			inventoryUI = FindObjectOfType<InventoryUI>(true);
			inspect = FindObjectOfType<Inspect>(true);

			SwitchState(State.Protagonist);
		}
		#endregion
	}
}