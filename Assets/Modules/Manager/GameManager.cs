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
			Prying,
			TV,
			Null,
			Console,
		}

		#region Inspector fields
		public float inventoryViewingDistance;
		public float inspectingDistance;
		#endregion

		#region Core fields
		[NonSerialized] public Protagonist protagonist;
		[NonSerialized] public InputManager input;
		[NonSerialized] public SceneChange sceneChange;
		public DialogueSystemController ds;
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
						break;
					case StateEnum.Protagonist:
						state = StateEnum.Protagonist;
						input.enabled = true;
						input.playerInput.SwitchCurrentActionMap("Protagonist");
						Cursor.lockState = CursorLockMode.Locked;
						//ui.SwitchTo(ui.aim);
						Time.timeScale = 1f;
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
					case StateEnum.Console:
						input.enabled = true;
						input.playerInput.SwitchCurrentActionMap("Console");
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
					State = StateEnum.Null;
					StartCoroutine(PryState());
				}
			}
		}

		public IEnumerator PryState()
		{	
			yield return new WaitForSeconds(2f);
			State = StateEnum.Prying;
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
				//State = StateEnum.Inventory;
			}
        }

		public void NullState()
        {
			State = StateEnum.Null;
        }

		public void ConsoleState(bool isConsoleState)
        {
            if (isConsoleState)
            {
				State = StateEnum.Console;
            }
			if (! isConsoleState)
            {
				State = StateEnum.Protagonist;
            }
        }
		public void ProtagonistState()
        {
			State = StateEnum.Protagonist;
        }
		#endregion

		#region Life cycle
		void Start() {
			protagonist = FindObjectOfType<Protagonist>(true);
			input = GetComponent<InputManager>();
			sceneChange = GetComponent<SceneChange>();


			State = StateEnum.Protagonist;
		}
		#endregion
	}
}
