using UnityEngine;

namespace Game {
	public class Protagonist : Character {
		#region Core members
		new Camera camera;
		CameraInteractor interactor;
		float eyeHangingOffset;
		#endregion

		#region Public interfaces
		public override void Rotate(Vector2 rotation) {
			base.Rotate(rotation);

			Vector3 cam = camera.transform.rotation.eulerAngles;
			cam.x += rotation.y;
			if(cam.x >= 180)
				cam.x -= 360;
			cam.x = Mathf.Clamp(cam.x, pitchRange.x, pitchRange.y);
			camera.transform.rotation = Quaternion.Euler(cam);
		}

		public override bool Crouching {
			get => base.Crouching;
			set {
				base.Crouching = value;
				Vector3 camPos = camera.transform.localPosition;
				camPos.y = Height - eyeHangingOffset;
				camera.transform.localPosition = camPos;
			}
		}

		public void Interact(bool active) {
			if(active)
				interactor.Activate();
			else
				interactor.Deactivate();
		}
		#endregion

		#region Life cycle
		protected new void Start() {
			base.Start();

			camera = GetComponentInChildren<Camera>();
			interactor = GetComponentInChildren<CameraInteractor>();

			eyeHangingOffset = height.y - camera.transform.localPosition.y;
			lastGroundHeight = transform.position.y;
		}
		#endregion
	}
}
