using System.Collections.Generic;
using UnityEngine;

public static class Utils {
	public static void DestroyAllChildren(this Transform root) {
		var children = new List<Transform>();
		for(int i = 0; i < root.childCount; ++i)
			children.Add(root.GetChild(i));
		foreach(Transform child in children)
			Object.Destroy(child.gameObject);
	}
}
