using Sandbox;
using System.Linq;
using WordBomb.Helpers;

namespace WordBomb.States
{
	public partial class WaitingState : State
	{
		public override string StateName => "WAITING";

		protected override void OnStart()
		{
			Log.Info( $"Started {StateName} State" );

			if ( Host.IsServer )
			{
				var players = Client.All.Select( ( client ) => client.Pawn as Player );

				foreach ( var player in players )
				{
					OnPlayerJoin( player );
				}
			}

			base.OnStart();
		}

		protected override void OnFinish()
		{
			base.OnFinish();
		}

		public override void OnPlayerJoin( Player player )
		{
			var stateHandler = StateHandler.Instance;
			if ( stateHandler == null ) return;

			if ( stateHandler.Players.Count >= Consts.MIN_PLAYERS_COUNT )
			{
				stateHandler.ChangeState( new PlayingState() );
			}

			base.OnPlayerJoin( player );
		}

		[ServerCmd( "wb_state_wait" )]
		public static void WaitState()
		{
			StateHandler.Instance?.ChangeState( new WaitingState() );
		}
	}
}
