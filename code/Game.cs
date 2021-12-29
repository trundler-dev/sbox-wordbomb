using Sandbox;
using WordBomb.States;

namespace WordBomb
{
	public partial class Game : Sandbox.Game
	{
		public static Game Instance => Current as Game;
		[Net] public StateHandler StateHandler { get; private set; }

		public Game()
		{
			if ( IsServer )
			{
				StateHandler = new();
			}
		}

		public override void ClientJoined( Client cl )
		{
			base.ClientJoined( cl );

			var player = new Player();
			cl.Pawn = player;

			StateHandler.OnPlayerJoin( player );
		}
	}
}
