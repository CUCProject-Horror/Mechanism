using UnityEngine;

namespace Game.Ui {
	[RequireComponent(typeof(UnityEngine.UI.Button))]
	public class Button : UiElement {
		#region Internal fields
		UnityEngine.UI.Button legacyButton;
		#endregion

		#region Public interface
		#endregion

		#region Life cycle
		void Start() {
			legacyButton = GetComponentInChildren<UnityEngine.UI.Button>();
			if(legacyButton)
				legacyButton.onClick.AddListener(OnUse);
		}
		#endregion
	}
}