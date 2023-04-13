using UnityEngine;
using System.Collections.Generic;

namespace Game {
	public class UiManager : MonoBehaviour {
		public UiBase aim, inventory;
		public Stack<UiBase> history = new Stack<UiBase>();
		public UiBase Current => history.Count == 0 ? null : history.Peek();

		public void ForwardTo(UiBase ui)
		{
			while(history.Contains(ui))
			{
				if (Current == ui)
					return;
				Back();
			}
			if (history.Count != 0)
				history.Peek()?.Deactivate();
			history.Push(ui);
			ui.Activate();
		}

		public void Back() {
			if(history.Count == 0)
				return;
			var top = history.Pop();
			top.Deactivate();
			if(history.Count != 0)
				Current?.Activate();
			if (Current == aim)
				GameManager.instance.State = GameManager.StateEnum.Protagonist;
		}

		public void Clear() {
			while(history.Count > 0)
				Back();
		}

		public void SwitchTo(UiBase ui) {
			if (Current == ui)
				return;
			Clear();
			ForwardTo(ui);
		}

		
	}
}