using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Game {
	public partial class CameraInteractor : MonoBehaviour {
		#region Inspector members
		public new Camera camera;
		public float maxDistance = 10;
		public bool raycastAll = false;
		#endregion

		#region Core members
		bool activity = false;
		[NonSerialized] public List<InteractableTarget> lastFocused;
		#endregion

		#region Public interfaces
		public bool Activity {
			get => activity;
			set {
				if(activity == value)
					return;
				activity = value;
				if(activity) {
					foreach(InteractableTarget target in lastFocused) {
						if(!target.focused  && target.gameObject.transform.parent.gameObject.GetComponent<DoubleTrigger>().canInteract)
							target.OnFocus(this);
						if(!target.activated && target.gameObject.transform.parent.gameObject.GetComponent<DoubleTrigger>().canInteract)
							target.OnActivate(this);
					}
				}
				else {
					foreach(InteractableTarget target in lastFocused) {
						if(target.activated)
							target.OnDeactivate(this);
						if(target.focused)
							target.OnBlur(this);
					}
				}
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
			Ray ray = camera.ScreenPointToRay(new Vector2(camera.pixelWidth, camera.pixelHeight) / 2);
			var hits = new List<RaycastHit>();
			if(raycastAll) {
				var hitsArr = Physics.RaycastAll(ray, maxDistance, 1 << 8);
				hits.AddRange(hitsArr);
			}
			else {
				RaycastHit hit;
				Physics.Raycast(ray, out hit, maxDistance, 1 << 8);
				if(hit.collider)
					hits.Add(hit);
			}
			var currentFocused = hits
				.Select((RaycastHit hit) => hit.collider?.GetComponent<InteractableTarget>())
				.Where((InteractableTarget usable) => usable != null)
				.ToList();
			foreach(InteractableTarget target in currentFocused) {
				if(!lastFocused.Contains(target))
					target.OnFocus(this);
			}
			foreach(InteractableTarget target in lastFocused) {
				if(!currentFocused.Contains(target)) {
					if(target.activated)
						target.OnDeactivate(this);
					target.OnBlur(this);
				}
			}
			lastFocused = currentFocused;
		}
	}
}