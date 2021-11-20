using Sandbox.UI;

namespace WordBomb.UI
{
	public partial class EndHud : Panel
	{

		public EndHud()
		{
			StyleSheet.Load( "/UI/Stylesheets/EndHud.scss" );
			Label label = AddChild<Label>( "winner" );

			var game = Game.Instance;
			if ( game == null ) return;

			var client = game.ActivePlayer.Client;

			label.Text = $"{client.Name} is the Winner!";
		}
	}
}
