using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game {
	public class Door : MonoBehaviour {
		#region Inspector fields
		[Range(.1f, 2)] public float speed = 1;
		public Animator animator;
		public List<Transform> rotatingPivots;
		#endregion

		#region Core fields
		float position = 0;
		float targetPosition;
		bool locked = false;

		IEnumerator positionSettingCoroutine = null;
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
			positionSettingCoroutine = null;
		}
		#endregion

		#region Public interfaces
		public bool Locked {
			get => locked;
			set {
				if(Locked == value)
					return;
				TargetPosition = 0;
				animator.SetBool("Locked", locked = value);
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
				if(positionSettingCoroutine != null)
					StopCoroutine(positionSettingCoroutine);
				StartCoroutine(positionSettingCoroutine = PositionSettingCoroutine());
			}
		}

		public float Position {
			get => position;
			set {
				value = Mathf.Clamp01(value);
				if(Locked && value != 0)
					return;
				animator.SetFloat("Position", position = value);
				foreach(Transform pivot in rotatingPivots) {
					Vector3 euler = pivot.localRotation.eulerAngles;
					euler.y = -position * 90;
					pivot.localRotation = Quaternion.Euler(euler);
				}
			}
		}

		public void Toggle() {
			TargetPosition = TargetPosition < .5f ? 1 : 0;
		}
		#endregion
	}
}