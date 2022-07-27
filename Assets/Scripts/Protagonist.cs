using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using NaughtyAttributes;

[RequireComponent(typeof(CharacterController))]
public class Protagonist : MonoBehaviour {
	[NonSerialized] public CharacterController controller;
	[NonSerialized] public new Camera camera;

	[Header("Movement")]
	[Range(1, 500)] public float sprintingSpeed;
	[Range(1, 500)] public float walkingSpeed;
	bool sprinting = false;
	Vector3 inputVelocity = Vector3.zero;

	[Header("Crouching")]
	[Range(1, 500)] public float crouchingSpeed;
	[MinMaxSlider(0, 2)] public Vector2 height;
	[NonSerialized] public float eyeHangingOffset;
	bool crouching = false;
	public float Height => crouching ? height.x : height.y;

	public float movementSpeed => crouching ? crouchingSpeed : sprinting ? sprintingSpeed : walkingSpeed;

	[Header("Orientation")]
	[Range(1, 10)] public float orientingSpeed;
	[MinMaxSlider(-90, 90)] public Vector2 pitchRange;
	Vector2 inputRotation = Vector2.zero;

	[Header("Falling")]
	[Range(1, 30)] public float fallingLimit = 5;
	[NonSerialized] public float lastGroundHeight;
	public UnityEvent onDieFalling;
	
	void UpdateMovement() {
		Vector3 velocity = inputVelocity * movementSpeed * Time.deltaTime;
		velocity = transform.localToWorldMatrix.MultiplyVector(velocity);
		controller.SimpleMove(velocity);
		if(controller.isGrounded) {
			if(lastGroundHeight - transform.position.y > fallingLimit)
				onDieFalling.Invoke();
			lastGroundHeight = transform.position.y;
		}
	}
	public void OnMove(InputValue value) {
		inputVelocity = value.Get<Vector2>();
		inputVelocity.z = inputVelocity.y;
		inputVelocity.y = 0;
	}

	public void OnSprint(InputValue value) {
		sprinting = value.isPressed;
	}

	public void OnCrouch(InputValue _) {
		crouching = !crouching;
		GetComponentInChildren<CapsuleCollider>().height = Height;
		controller.height = Height;
		controller.center = new Vector3(0, Height / 2, 0);
		Vector3 camPos = camera.transform.localPosition;
		camPos.y = Height - eyeHangingOffset;
		camera.transform.localPosition = camPos;
	}

	void UpdateOrientation() {
		Vector2 rotation = inputRotation * orientingSpeed * Time.deltaTime;
		rotation.y = -rotation.y;

		Vector3 body = transform.rotation.eulerAngles;
		body.y += rotation.x;
		transform.rotation = Quaternion.Euler(body);

		Vector3 cam = camera.transform.rotation.eulerAngles;
		cam.x += rotation.y;
		if(cam.x >= 180)
			cam.x -= 360;
		cam.x = Mathf.Clamp(cam.x, pitchRange.x, pitchRange.y);
		camera.transform.rotation = Quaternion.Euler(cam);
	}
	public void OnOrient(InputValue value) {
		inputRotation = value.Get<Vector2>();
	}

	public void Start() {
		camera = GetComponentInChildren<Camera>();
		controller = GetComponent<CharacterController>();
		Cursor.lockState = CursorLockMode.Locked;

		eyeHangingOffset = height.y - camera.transform.localPosition.y;
		lastGroundHeight = transform.position.y;
	}

	public void FixedUpdate() {
		InputSystem.Update();
		UpdateMovement();
		UpdateOrientation();
	}
}
