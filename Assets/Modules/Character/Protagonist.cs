using System;

namespace Game {
	public class Protagonist : Character {
		[NonSerialized] public CameraInteractor interactor;
		public void SetInteractorActivity(bool active) => interactor.Activity = active;

		new void Start() {
			base.Start();
			interactor = GetComponentInChildren<CameraInteractor>();
		}
	}
}