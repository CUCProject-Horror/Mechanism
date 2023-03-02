using UnityEngine;

namespace Game {
	public class Protagonist : Character {
		new Camera camera;
		float eyeHangingOffset;

		#region Public interfaces
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
				Vector3 camPos = camera.transform.localPosition;
				camPos.y = Height - eyeHangingOffset;
				camera.transform.localPosition = camPos;
			}
		}
		#endregion

		#region Life cycle
		protected new void Start() {
			base.Start();

			camera = GetComponentInChildren<Camera>();

			eyeHangingOffset = height.y - camera.transform.localPosition.y;
			lastGroundHeight = transform.position.y;
		}
		#endregion
	}
}