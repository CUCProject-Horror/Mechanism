using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;

namespace Game {
	public class GameManager : MonoBehaviour {
		#region Singleton
		public static GameManager instance;
		public GameManager() => instance = this;
		#endregion

		#region Inspector fields
		#endregion

		#region Core fields
		[NonSerialized] public UIManager ui;
		[NonSerialized] public Protagonist protagonist;
		[NonSerialized] public DialogueSystemController dialogue;

		State controlState = State.Protagonist;
		#endregion

		#region Public interfaces
		public enum State {
			Invalid = 0,
			Protagonist = 1,
			Inventory
		}
		public void SwitchState(State state) {
			switch(state) {
				case State.Invalid:
					controlState = State.Invalid;
					Cursor.lockState = CursorLockMode.None;
					protagonist.input.enabled = false;
					ui.Deactivate();
					break;
				case State.Protagonist:
					controlState = State.Protagonist;
					Cursor.lockState = CursorLockMode.Locked;
					protagonist.input.enabled = true;
					ui.SwitchTo(ui.aim);
					break;
				case State.Inventory:
					Cursor.lockState = CursorLockMode.None;
					protagonist.input.enabled = false;
					ui.SwitchTo(ui.inventory);
					break;
			}
		}

		public void CloseUI() => SwitchState(controlState);
		public void OpenInventory() => SwitchState(State.Inventory);
		#endregion

		#region Life cycle
		void Start() {
			ui = GetComponent<UIManager>();
			protagonist = FindObjectOfType<Protagonist>();
			dialogue = GetComponent<DialogueSystemController>();

			SwitchState(State.Protagonist);
		}
		#endregion
	}
}