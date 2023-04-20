using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
	public static class Utils {
		public static void DestroyAllChildren(this Transform root) {
			var children = new List<Transform>();
			for(int i = 0; i < root.childCount; ++i)
				children.Add(root.GetChild(i));
			foreach(Transform child in children)
				Object.Destroy(child.gameObject);
		}

		public static T StringToEnum<T>(string name) where T : System.Enum {
			return (T)System.Enum.Parse(typeof(T), name);
		}

		/// <summary>
		/// Calculate scrolling position of element in a scroll rect
		/// </summary>
		/// <remarks>https://stackoverflow.com/questions/30766020/how-to-scroll-to-a-specific-element-in-scrollrect-with-unity-ui</remarks>
		public static Vector2 GetSnapToPositionToBringChildIntoView(this ScrollRect instance, RectTransform child) {
			Canvas.ForceUpdateCanvases();
			Vector2 viewportLocalPosition = instance.viewport.localPosition;
			Vector2 childLocalPosition = child.localPosition;
			Vector2 result = new Vector2(
				0 - (viewportLocalPosition.x + childLocalPosition.x),
				0 - (viewportLocalPosition.y + childLocalPosition.y)
			);
			return result;
		}
	}
}