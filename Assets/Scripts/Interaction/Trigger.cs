using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class Trigger : MonoBehaviour {
	[Tag] public string tagMask;
	public UnityEvent<Collider> onEnter;
	public UnityEvent<Collider> onExit;

	public void OnTriggerEnter(Collider other) {
		if(other.tag != tagMask)
			return;
		onEnter.Invoke(other);
	}

	public void OnTriggerExit(Collider other) {
		if(other.tag != tagMask)
			return;
		onExit.Invoke(other);
	}
}
