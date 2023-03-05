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
		public void OnMove(InputValue value) {
			Vector2 raw = value.Get<Vector2>();
			protagonist.inputVelocity = new Vector3 {
				x = raw.x,
				y = 0,
				z = raw.y
			};
		}

		public void OnSprint(InputValue value) {
			protagonist.Sprinting = value.isPressed;
		}

		public void OnCrouch(InputValue _) {
			protagonist.Crouching = !protagonist.Crouching;
		}

		public void OnOrient(InputValue value) {
			protagonist.inputRotation = value.Get<Vector2>();
		}

		public void OnInventory() {
			GameManager.instance.State = GameManager.StateEnum.Inventory;
		}

		public void OnInteract(InputValue value) {
			float raw = value.Get<float>();
			protagonist.Interact(raw > .5f);
		}

		#endregion

		#region Life cycle
		void Start() {
			playerInput = GetComponent<PlayerInput>();
		}
		#endregion
	}
}
