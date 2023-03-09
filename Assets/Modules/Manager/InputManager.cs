using UnityEngine;
using UnityEngine.InputSystem;

namespace Game {
	[RequireComponent(typeof(PlayerInput))]
	public class InputManager : MonoBehaviour {
		#region Core fields
		PlayerInput playerInput;
		Protagonist protagonist => GameManager.instance.protagonist;
		#endregion

		#region Input handler
		public bool canOrient = true;
		public void OnMove(InputValue value) {
			Vector2 raw = value.Get<Vector2>();
			protagonist.inputVelocity = new Vector3 {
				x = raw.x,
				y = 0,
				z = raw.y
			};
		}

		public void OnSprint(InputValue value) {
			//protagonist.Sprinting = value.isPressed;
		}

		public void OnCrouch(InputValue _) {
			//protagonist.Crouching = !protagonist.Crouching;
		}

		public void OnOrient(InputValue value)
		{
			Vector2 raw = value.Get<Vector2>();
			// Direction & dragging
			{
                CameraInteractor interactor = protagonist.interactor;
				if(interactor.Activity) {
					foreach(var interactable in interactor.lastFocused)
						interactable.OnDrag(interactor, raw);
				}
			}
			// Protagonist orientation
			if (canOrient)
			{
                protagonist.Rotate(raw);
			}
		}

		public void OnInventory() {
			GameManager.instance.State = GameManager.StateEnum.Inventory;
		}

		public void OnInteract(InputValue value) {
			float raw = value.Get<float>();
			protagonist.SetInteractorActivity(raw > .5f);
		}

		#endregion

		#region Life cycle
		void Start() {
			playerInput = GetComponent<PlayerInput>();
			canOrient = true;
		}
		#endregion
	}
}
