using UnityEngine;
using System.Collections.Generic;

namespace Game {
	public class UiManager : MonoBehaviour {
		public UiBase aim, inventory;
		public Stack<UiBase> history = new Stack<UiBase>();

		public void ForwardTo(UiBase ui) {
			history.Push(ui);
			ui.Activate();
		}

		public void Back() {
			if(history.Count == 0)
				return;
			var top = history.Pop();
			top.Deactivate();
		}

		public void Clear() {
			while(history.Count > 0)
				Back();
		}

		public void SwitchTo(UiBase ui) {
			Clear();
			ForwardTo(ui);
		}
	}
}