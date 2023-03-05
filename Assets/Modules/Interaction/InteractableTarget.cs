using UnityEngine;
using UnityEngine.Events;
using System;

namespace Game {
	public class InteractableTarget : MonoBehaviour {
		public UnityEvent<Component> onFocus;
		public UnityEvent<Component> onBlur;
		public UnityEvent<Component> onActivate;
		public UnityEvent<Component> onDeactivate;
		public UnityEvent<Component, Vector3> onDrag;

		[NonSerialized] public bool focused, activated;

		public void OnFocus(Component source)
		{
			focused = true;
			onFocus.Invoke(source);
		}
		public void OnBlur(Component source)
		{
			focused = false;
			onBlur.Invoke(source);
		}
		public void OnActivate(Component source)
		{
			activated = true;
			onActivate.Invoke(source);
		}
		public void OnDeactivate(Component source)
		{
			activated = false;
			onDeactivate.Invoke(source);
		}
		public void OnInteract(Component source) {
			OnActivate(source);
			OnDeactivate(source);
		}
		public void OnDrag(Component source, Vector3 direction) {
			onDrag.Invoke(source, direction);
		}

        void Start()
        {
			focused = false;
			activated = false;
        }
    }
}