using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Ui {
	public class ButtonElement : UiElement, ISelectHandler {
		#region Serialized fields
		public UnityEngine.UI.Button legacyButton;
		#endregion

		#region Public interface
		// Make Unity selection happy
		public void OnSelect(BaseEventData eventData) => OnSelect();
		#endregion

		#region Life cycle
		void Start() {
			if(!legacyButton)
				legacyButton = GetComponentInChildren<UnityEngine.UI.Button>();
			if(!legacyButton)
				Debug.LogError("Button element has not a legacy button reference");
			else {
				legacyButton.onClick.AddListener(OnUse);
			}
		}
		#endregion
	}
}