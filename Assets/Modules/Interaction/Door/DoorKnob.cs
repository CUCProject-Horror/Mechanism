using UnityEngine;

namespace Game {
	public class DoorKnob : MonoBehaviour {
		public enum Direction {
			DontCare, Push, Pull
		}

		#region Inspector fields
		public InteractableTarget target;
		public Direction direction;
		#endregion

		#region Core fields
		Door door;
		#endregion

		#region Life cycle
		void Start() {
			door = GetComponentInParent<Door>();

			target.onDrag.AddListener((Component source, Vector3 drag) => {
				float dragDiretion = drag.y;
				if(Mathf.Abs(dragDiretion) < .1f)
					return;
				Debug.Log(dragDiretion);
				if(dragDiretion == 0)
					return;
				if(direction != Direction.DontCare) {
					switch(direction) {
						case Direction.Push:
							if(dragDiretion < 0)
								return;
							break;
						case Direction.Pull:
							if(dragDiretion > 0)
								return;
							break;
					}
				}
				door?.Toggle();
			});
		}
		#endregion
	}
}