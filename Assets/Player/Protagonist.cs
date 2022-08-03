using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Protagonist : Player {
	new Camera camera;
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
	#endregion

	public GameObject aimUI;
	public bool Input {
		set {
			GetComponent<PlayerInput>().enabled = value;
			Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
			aimUI?.SetActive(value);
		}
	}

	public new void Start() {
		base.Start();

		camera = GetComponentInChildren<Camera>();
		camera.tag = "MainCamera";
		Input = true;

		eyeHangingOffset = height.y - camera.transform.localPosition.y;
		lastGroundHeight = transform.position.y;
	}

	public void FixedUpdate() {
		InputSystem.Update();

		Vector3 velocity = inputVelocity * movementSpeed * Time.deltaTime;
		velocity = transform.localToWorldMatrix.MultiplyVector(velocity);
		Move(velocity);

		Vector2 rotation = inputRotation * orientingSpeed * Time.deltaTime;
		rotation.y = -rotation.y;
		Rotate(rotation);
	}
}
