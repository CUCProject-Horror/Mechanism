using UnityEngine;
using UnityEngine.Audio;

namespace Game {
	[CreateAssetMenu(menuName = "Game/AudioManagerAgent")]
	public class AudioManagerAgent : ScriptableObject {
		AudioManager audio => AudioManager.instance;

		public void PlaySourceOnce(string clipArrayName)
        {

			AudioClip[] clipToPlay = audio.doorOpenAud;
			GameObject source =
			Instantiate(audio.normalizedAudioSource, audio.audioManager.transform);
			int randomIndex = Random.Range(0, clipToPlay.Length);

			source.GetComponent<AudioSource>().clip = clipToPlay[randomIndex];
			source.GetComponent<AudioSource>().Play();
        }
	}
}