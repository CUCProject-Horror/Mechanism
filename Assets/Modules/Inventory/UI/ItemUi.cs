namespace Game {
	public class ItemUi : UiBase {
		public override void Deactivate() {
			base.Deactivate();
			Destroy(gameObject);
		}
	}
}