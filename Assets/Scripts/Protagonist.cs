using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Protagonist : MonoBehaviour {
	[NonSerialized] public CharacterController controller;
	[Range(1, 10)] public float movementSpeed;
	[Range(1, 10)] public float orientingSpeed;
	public new Camera camera;
	public InputAction movementInput;
	public InputAction orientationInput;
	Vector3 velocity;
	Vector2 orientationDelta;

	public void Move(Vector3 velocity) {
		velocity = transform.localToWorldMatrix.MultiplyVector(velocity);
		controller.SimpleMove(velocity);
	}

	public void Orient(Vector2 delta) {
		Vector3 body = transform.rotation.eulerAngles;
		body.y += delta.x;
		transform.rotation = Quaternion.Euler(body);

		Vector3 cam = camera.transform.rotation.eulerAngles;
		cam.x += delta.y;
		camera.transform.rotation = Quaternion.Euler(cam);
	}

	public void Start() {
		controller = GetComponent<CharacterController>();
		movementInput.performed += (InputAction.CallbackContext cb) => {
			Vector3 delta = cb.ReadValue<Vector2>();
			velocity = new Vector3(delta.x, 0, delta.y);
		};
		movementInput.canceled += (InputAction.CallbackContext cb) => velocity = Vector3.zero;
		movementInput.Enable();
		orientationInput.performed += (InputAction.CallbackContext cb) => {
			Vector2 delta = cb.ReadValue<Vector2>();
			delta.y = -delta.y;
			orientationDelta = delta;
		};
		orientationInput.canceled += (InputAction.CallbackContext cb) => {
			orientationDelta = Vector2.zero;
		};
		orientationInput.Enable();
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void Update() {
		Move(velocity * movementSpeed * 200 * Time.deltaTime);
		Orient(orientationDelta * orientingSpeed * Time.deltaTime);
	}
}
