using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSelector : MonoBehaviour {
	public new Camera camera;
	public float maxDistance = 10;
	public InputAction useInput;
	
	List<Usable> lastSelected;

	public void Use() {
		foreach(Usable usable in lastSelected)
			usable.BroadcastMessage("OnUse", this, SendMessageOptions.DontRequireReceiver);
	}

	public void Start() {
		if(camera == null)
			camera = GetComponent<Camera>();
		if(camera == null)
			Debug.LogWarning("Camera selector has no target camera");
		lastSelected = new List<Usable>();
		useInput.performed += (InputAction.CallbackContext _) => Use();
		useInput.Enable();
	}

	public void Update() {
		if(camera == null)
			return;
		Ray ray = camera.ScreenPointToRay(new Vector2(camera.pixelWidth, camera.pixelHeight) / 2);
		RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance);
		var currentSelected = hits
			.Select((RaycastHit hit) => hit.collider.GetComponent<Usable>())
			.Where((Usable usable) => usable != null)
			.ToList();
		currentSelected.ForEach((Usable usable) => {
			if(!lastSelected.Contains(usable))
				usable.onSelect.Invoke(this);
		});
		lastSelected.ForEach((Usable usable) => {
			if(!currentSelected.Contains(usable))
				usable.onDeselect.Invoke(this);
		});
		lastSelected = currentSelected;
	}
}
