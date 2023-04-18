using UnityEngine;

namespace Game.Ui {
	public class ButtonElement : UiElement {
		#region Serialized fields
		public UnityEngine.UI.Button legacyButton;
		#endregion

		#region Public interface
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