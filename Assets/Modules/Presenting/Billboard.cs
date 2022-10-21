using UnityEngine;

public class Billboard : MonoBehaviour {
	void FixedUpdate() {
		if(Camera.main == null)
			return;
		Vector3 offset = Camera.main.transform.position - transform.position;
		transform.rotation = Quaternion.LookRotation(offset);
	}
}
