using Sandbox;
using System.Collections.Generic;

namespace WordBomb.States
{
	public partial class StateHandler : BaseNetworkable
	{
		[Net] public State State { get; set; } = new WaitingState();
		public List<Player> Players { get; set; } = new();
		public static StateHandler Instance { get; set; }

		public StateHandler()
		{
			Instance = this;

			if ( Host.IsServer )
				State.Start();
		}

		public void OnPlayerJoin( Player player )
		{
			if ( !Players.Contains( player ) )
				Players.Add( player );
			State?.OnPlayerJoin( player );
		}

		public void RemovePlayer( Player player )
		{
			Players.Remove( player );
		}

		public void ChangeState( State state )
		{
			Assert.NotNull( state );

			State?.Finish();
			State = state;
			State?.Start();
		}

		[Event.Tick.Server]
		public static void Tick()
		{
			Instance?.State?.OnTick();
		}
	}
}
