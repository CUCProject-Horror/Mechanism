using UnityEngine;

public class PickupView : MonoBehaviour {
	public MeshFilter mesh;
	public new MeshRenderer renderer;

	public void View(Item item) {
		mesh.sharedMesh = item.mesh;
		renderer.sharedMaterial = item.material;
		gameObject.SetActive(true);
	}

	public void Start() {
		gameObject.SetActive(false);
	}
}
