using Sandbox;
using Sandbox.UI;
using WordBomb.States;

namespace WordBomb.UI
{
	public partial class GameArea : Panel
	{
		public Panel StateHud { get; set; }

		public GameArea()
		{
			StyleSheet.Load( "/UI/Stylesheets/GameArea.scss" );
			StateHud = AddChild<WaitingHud>();
		}

		public override void Tick()
		{

			var game = Game.Instance;
			if ( game == null ) return;

			var state = game.State;
			if ( state == null ) return;

			if ( state is WaitingState && StateHud is not WaitingHud )
			{
				StateHud.Delete();
				StateHud = AddChild<WaitingHud>();
			}
			else if ( state is PlayState && StateHud is not PlayingHud )
			{
				StateHud.Delete();
				StateHud = AddChild<PlayingHud>();
			}
			else if ( state is EndState && StateHud is not EndHud )
			{
				StateHud.Delete();
				StateHud = AddChild<EndHud>();
			}

			base.Tick();
		}
	}
}
