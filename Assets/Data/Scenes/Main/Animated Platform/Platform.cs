using UnityEngine;

public class Platform : MonoBehaviour {
	const string activeProperty = "Active";
	
	Animator animator;

	void Start() {
		animator = GetComponent<Animator>();
	}

	public void Activate() {
		animator.SetBool(activeProperty, true);
	}

	public void Deactivate() {
		animator.SetBool(activeProperty, false);
	}
}
