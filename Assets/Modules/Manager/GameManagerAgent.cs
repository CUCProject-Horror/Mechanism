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

		public void NullStateChange()
        {
			game.NullState();
        }

		public void ProtagonistStateChange()
        {
			game.ProtagonistState();
        }

		public void InventoryStateChange()
        {
			game.InventoryState();
        }

		public void ConsoleStateChange(bool isConsoleState)
        {
			game.ConsoleState(isConsoleState);
        }

		public void InspectItem(Item item) {
			game.InspectItem(item);
		}

		public void StartConversation(string name) 
		{
			game.ds.StopConversation();
			game.ds.StartConversation(name);
		}

		public void EndConversation()
        {
			game.ds.StopConversation();
        }

		public void RemoveItem(string itemName)
        {
			Inventory inventory = game.protagonist.inventory;

			inventory.Remove(itemName);
		}

		public void SwitchScene(int sceneNum)
        {
			game.sceneChange.sceneNum = sceneNum;
        }

		public void SwitchCutscene(int cutsceneNum)
        {
			game.sceneChange.cutsceneNum = cutsceneNum;
        }

	}
}
