using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour {
	public string tagMask = "";
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
