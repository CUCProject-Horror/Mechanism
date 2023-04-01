using UnityEngine.UI;

namespace Game
{
	public class ItemUi : UiBase {
		public Button closeButton;
		public Text text;

		Item item;

		public Item Item {
			get => item;
			set {
				item = value;
				text.text = item?.name;
			}
		}

		public override void Deactivate() {
			base.Deactivate();
			Destroy(gameObject);
		}
	}
}