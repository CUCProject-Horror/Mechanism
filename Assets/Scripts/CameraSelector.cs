using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraSelector : MonoBehaviour {
	public new Camera camera;
	public float maxDistance = 10;
	public InputAction useInput;
	
	List<Usable> lastSelected;

	public void Start() {
		camera = GetComponent<Camera>();
		lastSelected = new List<Usable>();
		useInput.performed += (InputAction.CallbackContext cb) =>
			lastSelected.ForEach((Usable usable) => usable.onUse.Invoke());
		useInput.Enable();
	}

	public void Update() {
		Ray ray = camera.ScreenPointToRay(new Vector2(camera.pixelWidth, camera.pixelHeight) / 2);
		RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance);
		var currentSelected = hits
			.Select((RaycastHit hit) => hit.collider.GetComponent<Usable>())
			.Where((Usable usable) => usable != null)
			.ToList();
		currentSelected.ForEach((Usable usable) => {
			if(!lastSelected.Contains(usable))
				usable.onSelect.Invoke();
		});
		lastSelected.ForEach((Usable usable) => {
			if(!currentSelected.Contains(usable))
				usable.onDeselect.Invoke();
		});
		lastSelected = currentSelected;
	}
}
