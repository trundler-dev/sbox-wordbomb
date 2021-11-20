using Sandbox;
using System.Linq;
using WordBomb.Logic;

namespace WordBomb.States
{
	public class PlayState : BaseState
	{
		public override string StateName => "PLAYING";

		protected override void OnStart()
		{
			base.OnStart();

			if ( Host.IsServer )
			{
				var players = Client.All.Select( ( client ) => client.Pawn as Player ).ToArray();

				Game.Instance.ActivePlayer = players[0];
				players[0].IsTurn = true;
				players[0].Prompt = PromptDictionary.GetRandomTwoLetterPrompt();
				Game.Instance.LivingPlayers = Client.All.Count;

				foreach ( var player in players )
				{
					player.TimeSince = 0;
					player.IsPlaying = true;
				}
			}
		}

		public override void OnPlayerLeave( Player player )
		{
			Game.Instance.LivingPlayers--;

			base.OnPlayerLeave( player );
		}

		public override void OnTick()
		{
			var player = Game.Instance.ActivePlayer;

			if ( player.IsTurn && player.TimeSince >= 5 )
			{
				Game.Instance.AssignNextPlayer();
			}
		}
	}
}
