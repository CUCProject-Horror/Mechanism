using UnityEngine;

public class Pickable : Usable {
	public void OnPick(Component source) {
		Protagonist protagonist = source.GetComponent<Protagonist>();
		if(protagonist == null)
			return;
		Destroy(gameObject);
	}

	public void Start() {
		onUse.AddListener(OnPick);
	}
}
