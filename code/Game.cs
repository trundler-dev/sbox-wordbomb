using Sandbox;

namespace WordBomb
{
	public partial class Game : Sandbox.Game
	{

		public static Game Instance
		{
			get => Current as Game;
		}

		public Game()
		{
		}

		public override void ClientJoined( Client cl )
		{
			var player = new Player();
			cl.Pawn = player;

			base.ClientJoined( cl );
		}
	}
}
