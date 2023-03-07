using UnityEngine;
using System;

namespace Game {
	public class Protagonist : Character {
		[NonSerialized] public CameraInteractor interactor;
		public void SetInteractorActivity(bool active) => interactor.Activity = active;

		public override void Rotate(Vector2 rotation) {
			base.Rotate(rotation);

			var camera = interactor.camera;
			Vector3 cam = camera.transform.rotation.eulerAngles;
			cam.x += rotation.y;
			if(cam.x >= 180)
				cam.x -= 360;
			cam.x = Mathf.Clamp(cam.x, -90, 90);
			camera.transform.rotation = Quaternion.Euler(cam);
		}

		new void Start() {
			base.Start();
			interactor = GetComponentInChildren<CameraInteractor>();
		}
	}
}