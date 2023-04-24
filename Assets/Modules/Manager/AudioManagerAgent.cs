using UnityEngine;
using UnityEngine.Audio;

namespace Game {
	[CreateAssetMenu(menuName = "Game/AudioManagerAgent")]
	public class AudioManagerAgent : ScriptableObject {
		public AudioManager audio => AudioManager.instance;


		public void PlaySourceOnce(AudioClipArray array)
        {
			audio.PlaySourceOnce(array);
        }
	}
}