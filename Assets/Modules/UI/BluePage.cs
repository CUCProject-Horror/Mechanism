using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

namespace Game.Ui {
	[ExecuteAlways]
	public class BluePage : UiPage {
		#region Serialized fields
		public Color foregroundColor, backgroundColor;
		public UiElement title;
		public Graphic background;
		public ButtonElement backButton;
		public ScrollRect scroll;
		public RectTransform entryListContainer;

		public GameObject entryButtonPrefab;
		public LayoutGroup entryList;

		public UnityEvent onEntrySelect;
		#endregion

		#region Public functions
		public void SetUpEntriesNagivation() {
			var children = UiElement.FindDirectChildren(entryList.transform as RectTransform).ToArray();
			for(var i = 0; i < children.Length; ++i) {
				var child = children[i];
				if(i == 0) {
					child.navigation.up ??= backButton;
					backButton.navigation.down ??= child;
				}
				if(i > 0)
					child.navigation.up ??= children[i - 1];
				if(i < children.Length - 1)
					child.navigation.down ??= children[i + 1];
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

		protected virtual void OnEntrySelect(UiElement entry) {
			// Update scrollbar value
			int i = entry.transform.GetSiblingIndex();
			int n = entryList.transform.childCount;
			float p = (float)i / (n - 1);
			p = 1 - p;
			scroll.verticalScrollbar.value = p;

			onEntrySelect?.Invoke();
		}

		protected override void OnSelect(UiElement element) {
			base.OnSelect(element);
			if(element.transform.parent == entryList.transform)
				OnEntrySelect(element);
		}
		#endregion
	}
}