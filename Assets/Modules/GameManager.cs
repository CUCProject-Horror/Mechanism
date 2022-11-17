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

		State controlState = State.Protagonist;
		#endregion

		#region Auxiliray
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
					inspect.ShowCloseButton = false;
					inspect.ViewingDistance = inventoryViewingDistance;
					ui.SwitchTo(ui.inventory);
					ui.Activate(ui.inspect);
					break;
				case State.Inspect:
					Cursor.lockState = CursorLockMode.None;
					protagonist.input.enabled = false;
					inspect.ShowCloseButton = true;
					inspect.ViewingDistance = inspectingDistance;
					ui.SwitchTo(ui.inspect);
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

		public void SetInspectItem(Item item) => inspect.SetItem(item);
		public void InspectItem(Item item) {
			SetInspectItem(item);
			SwitchState(State.Inspect);
		}
		#endregion

		#region Life cycle
		void Start() {
			ui = GetComponent<UIManager>();
			protagonist = FindObjectOfType<Protagonist>();
			dialogue = GetComponent<DialogueSystemController>();
			inspect = FindObjectOfType<Inspect>();

			SwitchState(State.Protagonist);
		}
		#endregion
	}
}