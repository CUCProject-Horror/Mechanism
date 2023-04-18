using UnityEngine;

namespace Game.Ui {
	[RequireComponent(typeof(BluePage))]
	public class PauseUi : MonoBehaviour {
		BluePage bluePage;

		void Start() {
			bluePage = GetComponent<BluePage>();
		}
	}
}