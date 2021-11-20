using Sandbox;
using Sandbox.UI;

namespace WordBomb.UI
{
	public partial class SubmissionTextEntry : TextEntry
	{
		public SubmissionTextEntry()
		{
			AddEventListener( "onsubmit", () => Submit() );
			AddClass( "hide" );
		}

		private void Submit()
		{
			string message = Text.Trim();
			Text = "";

			var game = Game.Instance;
			if ( game == null ) return;

			var player = Local.Pawn as Player;
			if ( player == null ) return;

			SubmitWord( message );
		}

		public override void OnButtonTyped( string button, KeyModifiers km )
		{
			UpdateLastTyped( Text );

			base.OnButtonTyped( button, km );
		}

		public override void Tick()
		{
			var player = Local.Pawn as Player;
			if ( player == null ) return;

			if ( player.IsTurn )
			{
				Focus();
			}
			else
			{
				Text = "";
			}

			SetClass( "hide", !player.IsTurn );

			base.Tick();
		}

		[ServerCmd]
		public static void UpdateLastTyped( string k )
		{
			var game = Game.Instance;
			if ( game == null ) return;

			if ( (ConsoleSystem.Caller.Pawn as Player) != game.ActivePlayer ) return;

			game.UpdateLastTyped( k );
		}

		[ServerCmd]
		public static void SubmitWord( string word )
		{
			var game = Game.Instance;
			if ( game == null ) return;

			if ( (ConsoleSystem.Caller.Pawn as Player) != game.ActivePlayer ) return;

			game.SubmitWord( word );
		}
	}
}
