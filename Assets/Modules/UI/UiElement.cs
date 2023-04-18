using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Game.Ui {
	public struct UiElementNavigation {
		public UiElement left, up, right, down;
	}

	[RequireComponent(typeof(RectTransform))]
	[ExecuteAlways]
	public class UiElement : MonoBehaviour {
		#region Internal fields
		Text legacyText;
		#endregion

		#region Serialized fields
		[SerializeField] bool selectable = true;
		public UiElementNavigation navigation;
		public UnityEvent onSelect;
		public UnityEvent onDeselect;
		public UnityEvent onUse;
		#endregion

		#region Core methods
		public static IEnumerable<UiElement> FindDirectChildren(RectTransform root) {
			foreach(RectTransform child in root) {
				if(child == null)
					continue;
				var ui = child.GetComponent<UiElement>();
				if(ui != null)
					yield return ui;
				foreach(var childUi in FindDirectChildren(child))
					yield return childUi;
			}
		}
		void TrySetLegacyText() {
			if(legacyText != null && legacyText.transform.IsChildOf(transform))
				return;
			legacyText = null;
			legacyText = GetComponentInChildren<Text>();
		}
		#endregion

		#region Public interfaces
		public RectTransform Transform => transform as RectTransform;
		public UiElement Parent {
			get {
				for(var t = transform; (t = t.parent) != null;) {
					var parent = t.GetComponent<UiElement>();
					if(parent)
						return parent;
				}
				return null;
			}
			set {
				if(value.Parent != Parent)
					throw new UnityException("Cannot set UI element parent across page");
				if(value.transform.IsChildOf(transform))
					throw new UnityException("Cannot set one UI element to be the child of its own child");
				transform.SetParent(value.transform);
			}
		}
		public IEnumerable<UiElement> DirectChildren => FindDirectChildren(Transform);
		public UiPage Page => GetComponentInParent<UiPage>();

		public virtual bool Selectable {
			get => selectable;
			set {
				foreach(var child in DirectChildren)
					child.Selectable = value;
				selectable = value;
			}
		}
		public bool Selected => Page?.SelectedElement == this;

		public string Text {
			get {
				TrySetLegacyText();
				return legacyText?.text;
			}
			set {
				TrySetLegacyText();
				if(legacyText)
					legacyText.text = value;
			}
		}

		public UiElement Navigate(Vector2 direction) {
			UiElement result = null;
			if(direction.magnitude != 0) {
				var d = direction.normalized * Mathf.Sqrt(2);
				if(Mathf.Abs(d.x) > 1)
					result = d.x < 0 ? navigation.left : navigation.right;
				else if(Mathf.Abs(d.y) > 1)
					result = d.y < 0 ? navigation.down : navigation.up;
			}
			if(!result || result == this)
				return this;
			if(result.isActiveAndEnabled) {
				if(Page)
					Page.SelectedElement = result;
				return result;
			}
			return result.Navigate(direction);
		}
		#endregion

		#region Message handlers
		public virtual void OnSelect() => onSelect?.Invoke();
		public virtual void OnDeselect() => onDeselect?.Invoke();
		public virtual void OnUse() => onUse?.Invoke();
		#endregion

		#region Life cycle
		void OnDisable() {
			if(Selected)
				Page.SelectedElement = null;
		}
		#endregion
	}
}