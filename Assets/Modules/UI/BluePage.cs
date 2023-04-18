using UnityEngine;
using UnityEngine.UI;

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

		#region Life cycle
		protected override void EditorUpdate() {
			base.EditorUpdate();
			if(background) {
				background.color = backgroundColor;
			}
		}
		#endregion
	}
}