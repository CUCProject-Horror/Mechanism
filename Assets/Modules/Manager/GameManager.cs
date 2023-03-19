using UnityEngine;
using System;

namespace Game {
	public class GameManager : MonoBehaviour {
		public static GameManager instance;
		public GameManager() => instance = this;


		public enum StateEnum {
			Invalid = 0,
			Protagonist = 1,
			Inventory,
			Prying,
			TV,
			Null,
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
		PlayerPry currentPrying;
		public VidController vid;

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
						input.playerInput.SwitchCurrentActionMap("Protagonist");
						Cursor.lockState = CursorLockMode.Locked;
						ui.SwitchTo(ui.aim);
						break;
					case StateEnum.Inventory:
						Cursor.lockState = CursorLockMode.None;
						input.enabled = false;
						if(inventoryUI.currentCat != null)
							inventoryUI.SwitchCategoryTab(inventoryUI.currentCat);
							input.playerInput.SwitchCurrentActionMap("UI");
						ui.SwitchTo(ui.inventory);
						break;
					case StateEnum.Prying:
						input.enabled = true;
						input.playerInput.SwitchCurrentActionMap("Pry");
						break;
					case StateEnum.TV:
						input.enabled = true;
						input.playerInput.SwitchCurrentActionMap("TV");
						break;
					case StateEnum.Null:
						input.enabled = true;
						input.playerInput.SwitchCurrentActionMap("Do Nothing");
						break;
				}
			}
		}

		public PlayerPry Prying {
			get => currentPrying;
			set {
				if (currentPrying == value)
					return;
				currentPrying?.Deactivate();
				State = StateEnum.Protagonist;
				if (currentPrying = value)
				{
					currentPrying.Activate();
					State = StateEnum.Prying;
				}
			}
		}

		public void TVState(int TVState)
        {
			if (TVState == 1)
			{
				State = StateEnum.TV;
			}
			else if(TVState == 2)
			{
				State = StateEnum.Protagonist;
			}
			else if(TVState == 3)
            {
				State = StateEnum.Inventory;
			}
        }

		public void NullState()
        {
			State = StateEnum.Null;
        }

		public void ProtagonistState()
        {
			State = StateEnum.Protagonist;
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
