using UnityEngine;
using UnityEditor;

namespace Game.Editor {
	[CustomEditor(typeof(UiManager))]
	public class UiManagerEditor : UnityEditor.Editor {
		public override void OnInspectorGUI() {
			var ui = target as UiManager;

			if(Application.isPlaying) {
				var enabled = GUI.enabled;
				GUI.enabled = false;
				EditorGUILayout.LabelField("Current State", ui.CurrentState);
				GUI.enabled = enabled;
			}

			base.OnInspectorGUI();
		}
	}
}