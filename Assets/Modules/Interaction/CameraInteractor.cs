using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game {
	public class CameraInteractor : MonoBehaviour {
		public new Camera camera;
		public float maxDistance = 10;

		List<InteractableTarget> lastFocused;

		public void Activate() {
			foreach(InteractableTarget target in lastFocused)
				target.BroadcastMessage("OnActivate", this, SendMessageOptions.DontRequireReceiver);
		}

		public void Deactivate() {
			foreach(InteractableTarget target in lastFocused)
				target.BroadcastMessage("OnDeactivate", this, SendMessageOptions.DontRequireReceiver);
		}

		public void Start() {
			if(camera == null)
				camera = GetComponent<Camera>();
			lastFocused = new List<InteractableTarget>();
		}

		public void Update() {
			if(camera == null)
				return;
			Ray ray = camera.ScreenPointToRay(new Vector2(camera.pixelWidth, camera.pixelHeight) / 2);
			RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance);
			var currentSelected = hits
				.Select((RaycastHit hit) => hit.collider.GetComponent<InteractableTarget>())
				.Where((InteractableTarget usable) => usable != null)
				.ToList();
			currentSelected.ForEach((InteractableTarget usable) => {
				if(!lastFocused.Contains(usable))
					usable.onFocus.Invoke(this);
			});
			lastFocused.ForEach((InteractableTarget usable) => {
				if(!currentSelected.Contains(usable))
					usable.onBlur.Invoke(this);
			});
			lastFocused = currentSelected;
		}
	}
}