using UnityEngine;
using UnityEngine.Audio;

namespace Game {
	public class AudioManager : MonoBehaviour {
		public static AudioManager instance;

		void Start() {
			instance = this;
		}

		#region Core fields
		public GameObject audioManager;
		public GameObject normalizedAudioSource;
        #endregion
    }
}