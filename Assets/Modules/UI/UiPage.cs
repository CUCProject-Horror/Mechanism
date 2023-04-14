using UnityEngine;

namespace Game.Ui {
	[RequireComponent(typeof(RectTransform))]
	public class UiPage : MonoBehaviour {
		#region Fields
		UiElement selectedElement;
		#endregion

		#region Core methods
		#endregion

		#region Public interfaces
		public UiElement SelectedElement {
			get => selectedElement;
			set {
				if(selectedElement == value)
					return;
				selectedElement?.SendMessage("OnDeselect");
				selectedElement = value;
				selectedElement?.SendMessage("OnSelect");
			}
		}

		public void Use() => selectedElement?.SendMessage("OnUse");
		#endregion

		#region Life cycle
		#endregion
	}
}