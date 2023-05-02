using UnityEngine;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections;

namespace Game {
	public class GameManager : MonoBehaviour {
		public static GameManager instance;
		public GameManager() => instance = this;
		public StateEnum currentState;

		public enum StateEnum {
			Invalid = 0,
			Protagonist = 1,
			Prying,
			TV,
			UI,
		}

		#region Internal fields
		[NonSerialized] public Protagonist protagonist;
		[NonSerialized] public InputManager input;
		[NonSerialized] public SceneChange sceneChange;
		public DialogueSystemController ds;
		PlayerPry currentPrying;
		public VidController vid;

		StateEnum state = StateEnum.Protagonist;
		#endregion

		#region Serialized fields
		public float inventoryViewingDistance;
		public float inspectingDistance;
		public UiManager ui;
		#endregion

		#region Internal functions
		void SwitchState(StateEnum value) {
			// Exiting
			switch(state) {
				case StateEnum.Invalid:
					input.enabled = false;
					break;
			}
			// Entering
			switch(value) {
				case StateEnum.Invalid:
					state = StateEnum.Invalid;
					Cursor.lockState = CursorLockMode.None;
					input.enabled = false;
					break;
				case StateEnum.Protagonist:
					state = StateEnum.Protagonist;
					input.playerInput.SwitchCurrentActionMap("Protagonist");
					Cursor.lockState = CursorLockMode.Locked;
					Time.timeScale = 1f;
					break;
				case StateEnum.Prying:
					input.playerInput.SwitchCurrentActionMap("Pry");
					break;
				case StateEnum.TV:
					input.playerInput.SwitchCurrentActionMap("TV");
					break;
				case StateEnum.UI:
					Time.timeScale = 0f;
					input.playerInput.SwitchCurrentActionMap("UI");
					break;
			}
			state = value;
		}

		IEnumerator StartPryCoroutine() {
			yield return new WaitForSeconds(2f);
			State = StateEnum.Prying;
		}
		#endregion

		#region Public interfaces
		public StateEnum State {
			get => state;
			set => SwitchState(value);
		}

		public PlayerPry Prying {
			get => currentPrying;
			set {
				if(currentPrying == value)
					return;
				currentPrying?.Deactivate();
				State = StateEnum.Protagonist;
				if(currentPrying = value) {
					currentPrying.Activate();
					State = StateEnum.Invalid;
					StartCoroutine(StartPryCoroutine());
				}
			}
		}

		public void ChangeInputState(int stateToChange) {
			State = (StateEnum)stateToChange;
		}

		public void TVState(int TVState) {
			if(TVState == 1) {
				State = StateEnum.TV;
			}
			else if(TVState == 2) {
				State = StateEnum.Protagonist;
			}
			else if(TVState == 3) {
				State = StateEnum.UI;
			}
		}

		public void OpenPauseMenu() {
			if(State == StateEnum.UI)
				return;
			State = StateEnum.UI;
			ui.Open(ui.pauseUi.Bp);
		}

		public void OpenInventoryDirectly() {
			OpenPauseMenu();
			ui.Open(ui.inventoryUi.Bp);
		}
		#endregion

		#region Life cycle
		void Start() {
			protagonist = FindObjectOfType<Protagonist>(true);
			input = GetComponent<InputManager>();
			sceneChange = GetComponent<SceneChange>();


			State = StateEnum.Protagonist;
		}

		private void Update() {
			currentState = State;
		}
		#endregion
	}
}
