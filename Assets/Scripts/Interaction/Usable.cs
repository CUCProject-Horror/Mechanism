using UnityEngine;
using UnityEngine.Events;

public class Usable : MonoBehaviour {
	public UnityEvent<Component> onSelect;
	public UnityEvent<Component> onDeselect;
	public UnityEvent<Component> onUse;
}
