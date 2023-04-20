using UnityEngine;
using UnityEngine.UI;

namespace Game.Ui {
	public class BluePageEntryButton : ButtonElement {
		#region Serialized fields
		public Graphic selectionIcon;
		public Text entryText;
		public Graphic textBackground;
		#endregion

		#region Internal functions
		BluePage Bp => Page as BluePage;
		#endregion

		#region Public interface
		public override void OnSelect() {
			base.OnSelect();
			selectionIcon.enabled = true;
			entryText.color = Bp.backgroundColor;
			textBackground.color = Bp.foregroundColor;
		}

		public override void OnDeselect() {
			base.OnDeselect();
			selectionIcon.enabled = false;
			entryText.color = Bp.foregroundColor;
			textBackground.color = new Color(0, 0, 0, 0);
		}
		#endregion

		#region Life cycle
		#endregion
	}
}