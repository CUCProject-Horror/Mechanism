using UnityEngine;
using UnityEngine.Video;

namespace Game {
	[CreateAssetMenu(menuName = "Game/GameManagerAgent")]
	public class GameManagerAgent : ScriptableObject {
		public GameManager game => GameManager.instance;
		
		public bool protagonistCanOrient {
			set => game.input.canOrient = value;
		}

		public void OnVidItemView(VideoClip thisClip)
        {
			game.vid.gameObject.SetActive(true);
			game.vid.PlayVidInBag(thisClip);
			game.vid.isInventory = true;
        }

		public void Pry(PlayerPry pry) {
			game.Prying = pry;
		}

		public void TVStateChange(int state)
        {
			game.TVState(state);
        }
	}
}