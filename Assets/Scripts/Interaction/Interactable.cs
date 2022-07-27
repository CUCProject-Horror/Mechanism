using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class Interactable : MonoBehaviour {
	[Tag] public string tagMask;
	public SpriteRenderer indicator;

	[Header("Outer")]
	public Sprite outerIndicator;
	public Trigger outerTrigger;

	[Header("Inner")]
	public Sprite innerIndicator;
	public Trigger innerTrigger;

	bool canInteract = false;
	public UnityEvent onInteract;

	public void Start() {
		outerTrigger.tagMask = tagMask;
		outerTrigger.onEnter.AddListener((Collider other) => OnOuterChange(true));
		outerTrigger.onExit.AddListener((Collider other) => OnOuterChange(false));

		innerTrigger.tagMask = tagMask;
		innerTrigger.onEnter.AddListener((Collider other) => OnInnerChange(true));
		innerTrigger.onExit.AddListener((Collider other) => OnInnerChange(false));

		indicator.enabled = false;
	}

	public void OnOuterChange(bool enter) {
		indicator.sprite = enter ? outerIndicator : null;
		indicator.enabled = enter;
	}

	public void OnInnerChange(bool enter) {
		indicator.sprite = enter ? innerIndicator : outerIndicator;
		canInteract = enter;
	}

	public void OnInteract() {
		if(!canInteract)
			return;
		onInteract.Invoke();
		Debug.Log("Interact");
	}
}
