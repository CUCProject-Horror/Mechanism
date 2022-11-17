using UnityEngine;
using UnityEngine.Events;

namespace Game {
	public class Usable : MonoBehaviour {
		public UnityEvent<Component> onSelect;
		public UnityEvent<Component> onDeselect;
		public UnityEvent<Component> onUse;

		public void OnSelect(Component source) => onSelect.Invoke(source);
		public void OnDeselect(Component source) => onDeselect.Invoke(source);
		public void OnUse(Component source) => onUse.Invoke(source);
	}
}