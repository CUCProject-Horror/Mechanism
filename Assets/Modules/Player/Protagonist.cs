using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Protagonist : Player {
	public static Protagonist instance;
	public Protagonist() {
		instance = this;
	}

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

	public void OnInventory() {
		InventoryUI.instance.Open();
	}
	#endregion

	public GameObject aimUI;
	PlayerInput input;
	public bool Input {
		set {
			input.enabled = value;
			if(value) {
				UI = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
			aimUI?.SetActive(value);
		}
	}

	public bool UI {
		set {
			if(value) {
				Input = false;
				Cursor.lockState = CursorLockMode.None;
			}
		}
	}

	public new void Start() {
		base.Start();

		camera = GetComponentInChildren<Camera>();
		camera.tag = "MainCamera";
		input = GetComponent<PlayerInput>();
		Input = true;

		eyeHangingOffset = height.y - camera.transform.localPosition.y;
		lastGroundHeight = transform.position.y;
	}

	public void Update() {
		Vector3 velocity = inputVelocity * movementSpeed * Time.deltaTime;
		velocity = transform.localToWorldMatrix.MultiplyVector(velocity);
		Move(velocity);

		Vector2 rotation = inputRotation * orientingSpeed * Time.deltaTime;
		rotation.y = -rotation.y;
		Rotate(rotation);
	}
}
