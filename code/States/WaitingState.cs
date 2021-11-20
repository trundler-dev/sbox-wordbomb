using Sandbox;
using System.Linq;

namespace WordBomb.States
{
	public class WaitingState : BaseState
	{
		public override string StateName => "WAITING";

		protected override void OnStart()
		{
			base.OnStart();

			if ( Host.IsServer )
			{
				var players = Client.All.Select( ( client ) => client.Pawn as Player );

				foreach ( var player in players )
				{
					OnPlayerJoin( player );
				}
			}
		}

		protected override void OnFinish()
		{
			base.OnFinish();
		}

		public override void OnPlayerJoin( Player player )
		{
			if ( PlayerList.Contains( player ) ) return;

			AddPlayer( player );
			player.IsPlaying = false;

			base.OnPlayerJoin( player );
		}
	}
}
