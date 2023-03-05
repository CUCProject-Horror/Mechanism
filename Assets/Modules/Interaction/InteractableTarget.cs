using UnityEngine;
using UnityEngine.Events;

namespace Game {
	public class InteractableTarget : MonoBehaviour {
		public UnityEvent<Component> onFocus;
		public UnityEvent<Component> onBlur;
		public UnityEvent<Component> onActivate;
		public UnityEvent<Component> onDeactivate;
		public UnityEvent<Component, Vector3> onDrag;

		public void OnFocus(Component source) => onFocus.Invoke(source);
		public void OnBlur(Component source) => onBlur.Invoke(source);
		public void OnActivate(Component source) => onActivate.Invoke(source);
		public void OnDeactivate(Component source) => onDeactivate.Invoke(source);
		public void OnInteract(Component source) {
			OnActivate(source);
			OnDeactivate(source);
		}
		public void OnDrag(Component source, Vector3 direction) {
			onDrag.Invoke(source, direction);
		}
	}
}