using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Game.Ui {
	[RequireComponent(typeof(RectTransform))]
	[ExecuteAlways]
	public class UiPage : MonoBehaviour {
		#region Internal fields
		UiElement previouslySelectedElement;
		bool selectable = true;
		GraphicRaycaster raycaster;
		#endregion

		#region Serialized fields
		[SerializeField] UiElement selectedElement;
		public UnityEvent onOpen;
		public UnityEvent onClose;
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

		protected virtual void OnEnable() {
			onOpen?.Invoke();
		}

		protected virtual void OnDisable() {
			onClose?.Invoke();
		}

		void Start() {
			raycaster = GetComponentInParent<GraphicRaycaster>();
		}
		#endregion
	}
}