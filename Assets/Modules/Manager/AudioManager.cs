using UnityEngine;

namespace Game {
	public class AudioManager : MonoBehaviour {
		public static AudioManager instance;

		void Start() {
			instance = this;
		}
	}
}