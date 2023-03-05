using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game {
	public class CameraInteractor : MonoBehaviour {
		#region Inspector members
		public new Camera camera;
		public float maxDistance = 10;
		#endregion

		#region Core members
		bool activity = false;
		List<InteractableTarget> lastFocused;
		Vector3 lastDirection;
		#endregion

		#region Public interfaces
		public bool Activity {
			get => activity;
			set {
				if(activity == value)
					return;
				activity = value;
				string msg = activity ? "OnActivate" : "OnDeactivate";
				foreach(InteractableTarget target in lastFocused)
					target.BroadcastMessage(msg, this, SendMessageOptions.DontRequireReceiver);
			}
		}
		#endregion

		public void Start() {
			if(camera == null)
				camera = GetComponent<Camera>();
			lastFocused = new List<InteractableTarget>();
		}

		public void FixedUpdate() {
			if(camera == null)
				return;
			#region Raycast & focusing
			Ray ray = camera.ScreenPointToRay(new Vector2(camera.pixelWidth, camera.pixelHeight) / 2);
			RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance);
			var currentSelected = hits
				.Select((RaycastHit hit) => hit.collider.GetComponent<InteractableTarget>())
				.Where((InteractableTarget usable) => usable != null)
				.ToList();
			foreach(InteractableTarget target in currentSelected) {
				if(!lastFocused.Contains(target))
					target.onFocus.Invoke(this);
			}
			foreach(InteractableTarget target in lastFocused) {
				if(!lastFocused.Contains(target))
					target.onBlur.Invoke(this);
			}
			lastFocused = currentSelected;
			#endregion
			#region Direction & dragging
			if(Activity) {
				Vector3 direction = camera.transform.forward;
				Vector3 draggingWorldDirection = direction - lastDirection;
				foreach(InteractableTarget target in lastFocused) {
					Vector3 draggingLocalDirection = target.transform.worldToLocalMatrix.MultiplyVector(draggingWorldDirection);
					target.OnDrag(this, draggingLocalDirection);
				}
				lastDirection = direction;
			}
			#endregion
		}
	}
}