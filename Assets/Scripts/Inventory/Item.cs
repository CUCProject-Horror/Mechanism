using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Item")]
public class Item : ScriptableObject {
	public new string name;
	public Mesh mesh;
	public Material material;
}
