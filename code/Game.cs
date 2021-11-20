using Sandbox;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordBomb.Helpers;
using WordBomb.Logic;
using WordBomb.States;
using WordBomb.UI;

namespace WordBomb
{
	public partial class Game : Sandbox.Game
	{

		public Hud Hud { get; set; }
		[Net] public BaseState State { get; private set; }
		[Net] public Player ActivePlayer { get; set; }

		public int TurnsWithoutLoss { get; set; } = 0;
		
		public int LivingPlayers { get; set; } = 0;

		public WordDictionary Dict { get; set; } = new();

		public List<Client> clients { get; set; } = new();

		public static Game Instance
		{
			get => Current as Game;
		}

		public Game()
		{
			if ( IsServer )
			{
				Hud = new();
			}
		}

		public override void ClientJoined( Client cl )
		{
			var player = new Player();
			cl.Pawn = player;

			clients.Add( cl );
			State?.OnPlayerJoin( player );

			base.ClientJoined( cl );
		}

		public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
		{
			State?.OnPlayerLeave( Client.Pawn as Player );

			base.ClientDisconnect( cl, reason );
		}

		public override void PostLevelLoaded()
		{
			_ = StartSecondTimer();

			base.PostLevelLoaded();
		}

		/// <summary>
		/// Handle the end of a Player's turn and the start of a new turn.
		/// </summary>
		public void AssignNextPlayer()
		{
			var current = clients[0];
			var endingPlayer = current.Pawn as Player;
			endingPlayer.FinishTurn();

			clients.RemoveAt( 0 );
			clients.Add( current );

			// Skip clients whose player is not currently playing.
			if ( !(clients[0].Pawn as Player).IsPlaying )
			{
				AssignNextPlayer();
				return;
			}

			var startingPlayer = clients[0].Pawn as Player;
			startingPlayer.StartTurn();

			ActivePlayer = startingPlayer;
		}

		public bool SubmitWord( string word )
		{
			WordSubmission submission = new WordSubmission( word );

			if ( submission.SubmitWord() )
			{
				TurnsWithoutLoss++;
				AssignNextPlayer();
				return true;
			}

			TurnsWithoutLoss = 0;
			return false;
		}

		public void UpdateLastTyped( string k )
		{
			ActivePlayer.LastSubmittedWord = k;
		}

		public async Task StartSecondTimer()
		{
			while ( true )
			{
				await Task.DelaySeconds( 1 );
				OnSecond();
			}
		}

		private void OnSecond()
		{
			CheckMinimumPlayers();
			CheckWinCon();
			State?.OnSecond();
		}

		[Event( "tick" )]
		private void Tick()
		{
			State?.OnTick();
		}

		/// <summary>
		/// Changes the active state.
		/// </summary>
		/// <param name="state">The state to change to.</param>
		public void ChangeState(BaseState state)
		{
			Assert.NotNull( state );

			State?.Finish();
			State = state;
			State?.Start();
		}

		/// <summary>
		/// Starts the game if minimum players is reached.
		/// </summary>
		private void CheckMinimumPlayers()
		{
			if ( Client.All.Count >= Consts.MIN_PLAYERS_COUNT )
			{
				if ( State is WaitingState || State == null )
				{
					ChangeState( new PlayState() );
				}
			}
			else if ( State is not WaitingState )
			{
				ChangeState( new WaitingState() );
			}
		}

		/// <summary>
		/// If only one player is left, transition to End state.
		/// </summary>
		private void CheckWinCon()
		{
			if ( LivingPlayers == 1 && State is PlayState )
			{
				ChangeState( new EndState() );
			}
		}
	}
}
