using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour {
	public string tagMask = "Player";
	public RawImage indicator;
	public Texture outerIndicator;
	public Trigger outerTrigger;
	public Texture innerIndicator;
	public Trigger innerTrigger;
	bool canInteract = false;
	public UnityEvent onInteract;

	public void Start() {
		outerTrigger.tag = tagMask;
		outerTrigger.onEnter.AddListener((Collider other) => OnOuterChange(true));
		outerTrigger.onExit.AddListener((Collider other) => OnOuterChange(false));

		innerTrigger.tag = tagMask;
		innerTrigger.onEnter.AddListener((Collider other) => OnInnerChange(true));
		innerTrigger.onExit.AddListener((Collider other) => OnInnerChange(false));

		indicator.enabled = false;
	}

	public void OnOuterChange(bool enter) {
		indicator.texture = enter ? outerIndicator : null;
		indicator.enabled = enter;
	}

	public void OnInnerChange(bool enter) {
		indicator.texture = enter ? innerIndicator : outerIndicator;
		canInteract = enter;
	}

	public void OnInteract() {
		if(!canInteract)
			return;
		onInteract.Invoke();
		Debug.Log("Interact");
	}
}
