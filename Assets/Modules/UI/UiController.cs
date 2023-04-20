using UnityEngine;

namespace Game.Ui {
	[RequireComponent(typeof(UiPage))]
	public class UiController : MonoBehaviour {
		public UiPage Page => GetComponent<UiPage>();
		public BluePage Bp => Page as BluePage;
	}
}