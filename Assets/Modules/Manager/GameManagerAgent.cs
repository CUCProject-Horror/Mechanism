using UnityEngine;
using UnityEngine.Video;

namespace Game {
	[CreateAssetMenu(menuName = "Game/GameManagerAgent")]
	public class GameManagerAgent : ScriptableObject {
		public GameManager game => GameManager.instance;

		public void OnVidItemView(VideoClip thisClip) {
			game.vid.gameObject.SetActive(true);
			game.vid.PlayVidInBag(thisClip);
			game.vid.isInventory = true;
		}

		public void Pry(PlayerPry pry) {
			game.Prying = pry;
		}

		public void TVStateChange(int state) {
			game.TVState(state);
		}
		public void ProtagonistStateChange() => game.State = GameManager.StateEnum.Protagonist;

		public void ChangeInputState(int state)
		{
			game.ChangeInputState(state);
		}

		public void InventoryStateChange() {
			//game.InventoryState();
		}

		public void InspectItem(Item item) {
			//game.InspectItem(item);
		}

		public void StartConversation(string name) {
			game.ds.StopConversation();
			game.ds.StartConversation(name);
		}

		public void EndConversation() {
			game.ds.StopConversation();
		}

		public void SwitchScene(int sceneNum) {
			game.sceneChange.sceneNum = sceneNum;
		}

		public void SwitchCutscene(int cutsceneNum) {
			game.sceneChange.cutsceneNum = cutsceneNum;
		}

		public void Log(string message) {
			Debug.Log(message);
		}

		public void CanOrient(bool canOrient)
        {
			game.input.canOrient = canOrient;
        }

		public void UiChangeState(string thisState)
        {
			game.ui.AddState(thisState);
        }//打开某个UI加一个string来代表当前UIState

		public void UiRemoveCurrentState()
        {
			game.ui.RemoveLastState();
        }//关闭某个UI时删除最后的State

		public void RemoveItem(Item item)
        {
			game.protagonist.inventory.Deprive(item);
        }

	}
}
