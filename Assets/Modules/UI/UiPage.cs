using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;

namespace Game.Ui {
	[RequireComponent(typeof(RectTransform))]
	[ExecuteAlways]
	public class UiPage : MonoBehaviour {
		#region Internal fields
		UiElement previouslySelectedElement;
		UiElement lastSelectedBeforeClose;
		bool selectable = true;
		#endregion

		#region Serialized fields
		[SerializeField] UiElement selectedElement;
		public UnityEvent onOpen;
		public UnityEvent onClose;
		public enum OnOpenBehaviour {
			FirstElement,
			RestoreLastSelected,
			FixedElement,
		}
		public OnOpenBehaviour onOpenBehaviour;
		[ShowIf("onOpenBehaviour", OnOpenBehaviour.FixedElement)] public UiElement fixedElementToOpen;
		#endregion

		#region Core methods
		#endregion

		#region Public interfaces
		public UiElement SelectedElement {
			get => selectedElement;
			set {
				if(value == null)
					value = DirectChildren.FirstOrDefault(child => child.Selectable);
				if(!(value?.Selectable ?? false))
					return;
				if(value == previouslySelectedElement)
					return;
				if(previouslySelectedElement)
					previouslySelectedElement.OnDeselect();
				value = value?.isActiveAndEnabled ?? false ? value : null;
				previouslySelectedElement = selectedElement = value;
				if(value)
					lastSelectedBeforeClose = value;
				if(selectedElement)
					selectedElement.OnSelect();
			}
		}
		public IEnumerable<UiElement> DirectChildren => UiElement.FindDirectChildren(transform as RectTransform);

		public bool Selectable {
			get => selectable;
			set {
				if(selectable == value)
					return;
				foreach(var child in DirectChildren)
					child.Selectable = value;
				selectable = value;
			}
		}

		public void Navigate(Vector2 direction) {
			SelectedElement?.Navigate(direction);
		}

		public void Use() {
			if(Selectable)
				SelectedElement?.OnUse();
		}

		public void Close() {
			var ui = GameManager.instance.ui;
			if(ui.Current != this)
				return;
			ui.Close();
		}
		#endregion

		#region Life cycle
		protected virtual void EditorUpdate() {
			if(SelectedElement == null)
				SelectedElement = SelectedElement;
		}

		protected virtual void Update() {
			if(previouslySelectedElement != SelectedElement)
				SelectedElement = SelectedElement;
			if(!Application.isPlaying) {
				EditorUpdate();
				return;
			}
		}

		protected virtual void OnSelect(UiElement element) { }

		protected virtual void OnEnable() {
			onOpen?.Invoke();
			switch(onOpenBehaviour) {
				case OnOpenBehaviour.FirstElement:
					SelectedElement = DirectChildren.FirstOrDefault();
					break;
				case OnOpenBehaviour.RestoreLastSelected:
					if(lastSelectedBeforeClose == null)
						lastSelectedBeforeClose = DirectChildren.FirstOrDefault();
					SelectedElement = lastSelectedBeforeClose;
					break;
				case OnOpenBehaviour.FixedElement:
					SelectedElement = fixedElementToOpen;
					break;
			}
		}

		protected virtual void OnDisable() {
			SelectedElement = null;
			onClose?.Invoke();
		}
		#endregion
	}
}