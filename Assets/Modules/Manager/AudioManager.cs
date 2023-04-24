using UnityEngine;
using UnityEngine.Audio;

namespace Game {
	public class AudioManager : MonoBehaviour {
		public static AudioManager instance;

		public AudioManager() => instance = this;

		#region Internal Functions
		public void PlaySourceOnce(AudioClipArray array)
		{

			AudioClip[] clipToPlay = array.clipArray;
			GameObject source =
			Instantiate(normalizedAudioSource, audioManager.transform);
			int randomIndex = Random.Range(0, clipToPlay.Length);

			source.GetComponent<AudioSource>().clip = clipToPlay[randomIndex];
			source.GetComponent<AudioSource>().volume = array.volume;
			source.GetComponent<AudioSource>().Play();
		}
		#endregion

		#region Core fields
		public GameObject audioManager;
		public GameObject normalizedAudioSource;
        #endregion
    }
}