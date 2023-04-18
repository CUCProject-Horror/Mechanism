using UnityEngine;

namespace Game.Ui {
	[RequireComponent(typeof(RectTransform))]
	[ExecuteAlways]
	public class UiPage : MonoBehaviour {
		#region Internal fields
		UiElement previouslySelectedElement;
		#endregion

		#region Serialized fields
		public UiElement selectedElement;
		#endregion

		#region Core methods
		#endregion

		#region Public interfaces
		public UiElement SelectedElement {
			get => selectedElement;
			set {
				if(value == previouslySelectedElement)
					return;
				if(previouslySelectedElement) {
					previouslySelectedElement.OnDeselect();
				}
				previouslySelectedElement = selectedElement = value;
				if(selectedElement) {
					selectedElement.OnSelect();
				}
			}
		}

		public void Use() => selectedElement?.SendMessage("OnUse");
		#endregion

		#region Life cycle
		protected virtual void EditorUpdate() {
		}

		protected virtual void Update() {
			if(previouslySelectedElement != SelectedElement)
				SelectedElement = SelectedElement;
			if(!Application.isPlaying) {
				EditorUpdate();
				return;
			}
		}
		#endregion
	}
}