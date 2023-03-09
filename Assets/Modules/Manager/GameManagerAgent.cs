using UnityEngine;
using UnityEngine.Video;

namespace Game {
	[CreateAssetMenu(menuName = "Game/GameManagerAgent")]
	public class GameManagerAgent : ScriptableObject {
		public GameManager game => GameManager.instance;

		public bool protagonistCanOrient {
			set => game.input.canOrient = value;
		}

		public void OnVidItemView(VideoClip clipToPlay)
        {
			Debug.Log("View!");
			var player = FindObjectOfType<VidPlayer>();
			player.clipToPlay = clipToPlay;
			player.PlayVid();
        }
	}
}
