using UnityEngine;
using NaughtyAttributes;

namespace Game {
	[RequireComponent(typeof(CharacterController))]
	public class Character : MonoBehaviour {
		#region Inspector Properties
		#region Movability
		[Header("Movement")]
		[Range(1, 500)] public float sprintingSpeed;
		[Range(1, 500)] public float walkingSpeed;
		bool sprinting = false;

		[Header("Crouching")]
		[Range(1, 500)] public float crouchingSpeed;
		[MinMaxSlider(0, 2)] public Vector2 height;
		bool crouching = false;

		[Header("Orientation")]
		[Range(1, 10)] public float orientingSpeed;
		[MinMaxSlider(-90, 90)] public Vector2 pitchRange;

		[Header("Falling")]
		[Range(1, 30)] public float fallingLimit = 5;
		protected float lastGroundHeight;
		#endregion

		[Header("Inventory")]
		public Inventory inventory = new Inventory();
		#endregion

		#region Core fields
		protected CharacterController controller;
		#endregion

		#region Public interfaces
		public Vector3 inputVelocity = Vector3.zero;
		public Vector2 inputRotation = Vector2.zero;

		public float Height => crouching ? height.x : height.y;

		public virtual bool Sprinting {
			get => sprinting;
			set => sprinting = value;
		}

		public virtual bool Crouching {
			get => crouching;
			set {
				crouching = value;
				//GetComponentInChildren<CapsuleCollider>().height = Height;
				controller.height = Height;
				controller.center = new Vector3(0, Height / 2, 0);
			}
		}

		public float movementSpeed => crouching ? crouchingSpeed : sprinting ? sprintingSpeed : walkingSpeed;

		public virtual void Move(Vector3 velocity) {
			controller.SimpleMove(velocity);
			if(controller.isGrounded) {
                float fallingHeight = lastGroundHeight - transform.position.y;
                if (fallingHeight > fallingLimit)
					DieFalling();
				lastGroundHeight = transform.position.y;
			}
		}

		public virtual void Rotate(Vector2 rotation) {
			Vector3 body = transform.rotation.eulerAngles;
			body.y += rotation.x;
			transform.rotation = Quaternion.Euler(body);
		}

		public void DieFalling() {
			Debug.Log($"{gameObject.name} fell and died");
		}
		#endregion

		#region Life cycle
		protected void Start() {
			controller = GetComponent<CharacterController>();
		}

		void FixedUpdate() {
			Vector3 velocity = inputVelocity * movementSpeed * Time.fixedDeltaTime;
			velocity = transform.localToWorldMatrix.MultiplyVector(velocity);
			Move(velocity);

			Vector2 rotation = inputRotation * orientingSpeed * Time.fixedDeltaTime;
			rotation.y = -rotation.y;
			Rotate(rotation);
		}
		#endregion
	}
}
