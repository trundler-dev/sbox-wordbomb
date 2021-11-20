using Sandbox.UI;

namespace WordBomb.UI
{
	public partial class WaitingHud : Panel
	{
		private Label welcomeLabel;
		private Label waitingCountLabel;

		public WaitingHud()
		{
			StyleSheet.Load( "/UI/Stylesheets/WaitingHud.scss" );
			welcomeLabel = AddChild<Label>( "welcome" );
			welcomeLabel.Text = "Welcome to WordBomb!";
			waitingCountLabel = AddChild<Label>( "waiting" );
			waitingCountLabel.Text = "Starting soon...";
		}
	}
}
