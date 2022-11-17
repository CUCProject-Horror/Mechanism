using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace Game {
	[RequireComponent(typeof(PlayerInput))]
	public class Protagonist : Player {
		new Camera camera;
		[NonSerialized] public PlayerInput input;
		float eyeHangingOffset;

		public new void Rotate(Vector2 rotation) {
			base.Rotate(rotation);

			Vector3 cam = camera.transform.rotation.eulerAngles;
			cam.x += rotation.y;
			if(cam.x >= 180)
				cam.x -= 360;
			cam.x = Mathf.Clamp(cam.x, pitchRange.x, pitchRange.y);
			camera.transform.rotation = Quaternion.Euler(cam);
		}

		public new bool Crouching {
			get => base.Crouching;
			set {
				base.Crouching = value;
				GetComponentInChildren<CapsuleCollider>().height = Height;
				controller.height = Height;
				controller.center = new Vector3(0, Height / 2, 0);
				Vector3 camPos = camera.transform.localPosition;
				camPos.y = Height - eyeHangingOffset;
				camera.transform.localPosition = camPos;
			}
		}

		#region Input Handling
		Vector3 inputVelocity = Vector3.zero;
		Vector2 inputRotation = Vector2.zero;

		public void OnMove(InputValue value) {
			inputVelocity = value.Get<Vector2>();
			inputVelocity.z = inputVelocity.y;
			inputVelocity.y = 0;
		}

		public void OnSprint(InputValue value) {
			Sprinting = value.isPressed;
		}

		public void OnCrouch(InputValue _) {
			Crouching = !Crouching;
		}

		public void OnOrient(InputValue value) {
			inputRotation = value.Get<Vector2>();
		}

		public void OnInteract() {
			CameraSelector selector = GetComponentInChildren<CameraSelector>();
			if(selector == null)
				return;
			selector.Use();
		}

		public void OnInventory() {
			GameManager.instance.OpenInventory();
		}
		#endregion

		#region Life cycle
		protected new void Start() {
			base.Start();

			camera = GetComponentInChildren<Camera>();
			input = GetComponent<PlayerInput>();

			eyeHangingOffset = height.y - camera.transform.localPosition.y;
			lastGroundHeight = transform.position.y;
		}

		void Update() {
			Vector3 velocity = inputVelocity * movementSpeed * Time.deltaTime;
			velocity = transform.localToWorldMatrix.MultiplyVector(velocity);
			Move(velocity);

			Vector2 rotation = inputRotation * orientingSpeed * Time.deltaTime;
			rotation.y = -rotation.y;
			Rotate(rotation);
		}
		#endregion
	}
}