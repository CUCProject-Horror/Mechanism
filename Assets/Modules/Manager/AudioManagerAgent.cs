using UnityEngine;

namespace Game {
	[CreateAssetMenu(menuName = "Game/AudioManagerAgent")]
	public class AudioManagerAgent : ScriptableObject {
		AudioManager audio => AudioManager.instance;
	}
}