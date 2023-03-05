using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game {
	public class Door : MonoBehaviour {
		#region Inspector fields
		[Range(.1f, 2)] public float speed = 1;
		public List<Transform> rotatingPivots;
		public DoorKnob frontKnob, backKnob;
		#endregion

		#region Core fields
		float position = 0;
		float targetPosition;
		bool locked = false;

		IEnumerator workingCoroutine = null;
		#endregion

		#region Core methods
		IEnumerator PositionSettingCoroutine() {
			while(position != targetPosition) {
				float step = speed * Time.fixedDeltaTime;
				float delta = targetPosition - position;
				if(Mathf.Abs(delta) < step) {
					Position = targetPosition;
					break;
				}
				Position += Mathf.Sign(delta) * step;
				yield return new WaitForFixedUpdate();
			}
			workingCoroutine = null;
		}
		#endregion

		#region Public interfaces
		public bool Open => Position > .5f;

		public bool Locked {
			get => locked;
			set {
				if(Locked == value)
					return;
				TargetPosition = 0;
				locked = value;
			}
		}

		public float TargetPosition {
			get => targetPosition;
			set {
				value = Mathf.Clamp01(value);
				if(Locked && value != 0) {
					Debug.Log("Door locked. Can't open.");
					return;
				}
				targetPosition = value;
				if(workingCoroutine != null)
					StopCoroutine(workingCoroutine);
				StartCoroutine(workingCoroutine = PositionSettingCoroutine());
			}
		}

		public float Position {
			get => position;
			set {
				value = Mathf.Clamp01(value);
				if(Locked && value != 0)
					return;
				position = value;

				// Rotate door panels
				foreach(Transform pivot in rotatingPivots) {
					Vector3 euler = pivot.localRotation.eulerAngles;
					euler.y = position * 90;
					pivot.localRotation = Quaternion.Euler(euler);
				}

				// Activate/deactivate door knobs with respect to door position
				if(frontKnob)
					frontKnob.gameObject.SetActive(!Open);
				if(backKnob)
					backKnob.gameObject.SetActive(Open);
			}
		}

		public void Toggle() {
			TargetPosition = TargetPosition < .5f ? 1 : 0;
		}
		#endregion

		#region Life cycle
		void Start() {
			Position = 0;
		}
		#endregion
	}
}