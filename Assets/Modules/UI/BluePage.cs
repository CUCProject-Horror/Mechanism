using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Game.Ui {
	[ExecuteAlways]
	public class BluePage : UiPage {
		#region Serialized fields
		public Color foregroundColor, backgroundColor;
		public Graphic background;
		public ButtonElement backButton;

		public GameObject entryButtonPrefab;
		public LayoutGroup entryList;
		#endregion

		#region Internal functions
		void SetUpEntriesNagivation() {
			var children = UiElement.FindDirectChildren(entryList.transform as RectTransform).ToArray();
			for(var i = 0; i < children.Length; ++i) {
				var child = children[i];
				if(i == 0) {
					child.navigation.up = backButton;
					backButton.navigation.down = child;
				}
				if(i > 0)
					child.navigation.up = children[i - 1];
				if(i < children.Length - 1)
					child.navigation.down = children[i + 1];
			}
		}
		#endregion

		#region Life cycle
		protected override void OnEnable() {
			SetUpEntriesNagivation();
			base.OnEnable();
		}

		protected override void EditorUpdate() {
			base.EditorUpdate();
			if(background) {
				background.color = backgroundColor;
			}
		}
		#endregion
	}
}