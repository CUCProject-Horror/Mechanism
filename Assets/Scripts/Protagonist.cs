using System;
using UnityEngine;
using UnityEngine.InputSystem;
using NaughtyAttributes;

[RequireComponent(typeof(CharacterController))]
public class Protagonist : MonoBehaviour {
	// Inspector properties

	[Header("Movement")]
	[Range(1, 300)] public float runningSpeed;
	[Range(1, 300)] public float walkingSpeed;
	[Range(1, 300)] public float crouchingSpeed;
	bool running = false;
	bool crouching = false;
	public float movementSpeed => crouching ? crouchingSpeed : running ? runningSpeed : walkingSpeed;
	public InputAction movementInput;
	public InputAction runningInput;
	[MinMaxSlider(0, 2)] public Vector2 height;
	[NonSerialized] public float eyeHangingOffset;
	public float Height => crouching ? height.x : height.y;
	public InputAction crouchingInput;

	[Header("Orientation")]
	[Range(1, 10)] public float orientingSpeed;
	[MinMaxSlider(-90, 90)] public Vector2 pitchRange;
	public InputAction orientationInput;

	[Header("Misc")]
	[Range(1, 30)] public float fallingLimit = 5;
	[NonSerialized] public float lastGroundHeight;
	
	// Private properties

	[NonSerialized] public CharacterController controller;
	[NonSerialized] public new Camera camera;

	public void Move(Vector3 velocity) {
		velocity = transform.localToWorldMatrix.MultiplyVector(velocity);
		controller.SimpleMove(velocity);
	}

	public void SetRunning(bool value) {
		running = value;
	}

	public void SetCrouching(bool value) {
		crouching = value;
		GetComponentInChildren<CapsuleCollider>().height = Height;
		controller.height = Height;
		controller.center = new Vector3(0, Height / 2, 0);
		Vector3 camPos = camera.transform.localPosition;
		camPos.y = Height - eyeHangingOffset;
		camera.transform.localPosition = camPos;
	}

	public void Orient(Vector2 delta) {
		Vector3 body = transform.rotation.eulerAngles;
		body.y += delta.x;
		transform.rotation = Quaternion.Euler(body);

		Vector3 cam = camera.transform.rotation.eulerAngles;
		cam.x += delta.y;
		if(cam.x >= 180)
			cam.x -= 360;
		cam.x = Mathf.Clamp(cam.x, pitchRange.x, pitchRange.y);
		camera.transform.rotation = Quaternion.Euler(cam);
	}

	public void DieFalling() {
		Debug.Log("Protagonist fell from high and died.");
	}

	public void Start() {
		camera = GetComponentInChildren<Camera>();
		controller = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;

		movementInput.Enable();
		runningInput.Enable();
		runningInput.performed += (InputAction.CallbackContext cb) => SetRunning(cb.ReadValue<float>() >= .5f);
		crouchingInput.Enable();
		crouchingInput.performed += (InputAction.CallbackContext cb) => SetCrouching(!crouching);
		eyeHangingOffset = height.y - camera.transform.position.y;
		SetCrouching(false);
		orientationInput.Enable();

		lastGroundHeight = transform.position.y;
	}

	public void FixedUpdate() {
		Vector3 velocity = movementInput.ReadValue<Vector2>();
		Vector3 inputVelocity = new Vector3(velocity.x, 0, velocity.y);
		Move(inputVelocity * movementSpeed * Time.deltaTime);

		Vector2 rotation = orientationInput.ReadValue<Vector2>();
		rotation.y = -rotation.y;
		Vector2 inputRotation = rotation;
		Orient(inputRotation * orientingSpeed * Time.deltaTime);

		if(controller.isGrounded) {
			if(lastGroundHeight - transform.position.y > fallingLimit)
				DieFalling();
			lastGroundHeight = transform.position.y;
		}
	}
}
