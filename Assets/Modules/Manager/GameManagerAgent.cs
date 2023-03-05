using UnityEngine;

namespace Game {
	[CreateAssetMenu(menuName = "Game/GameManagerAgent")]
	public class GameManagerAgent : ScriptableObject {
		public GameManager game => GameManager.instance;

		public bool protagonistCanOrient {
			set => game.input.canOrient = value;
		}
	}
}