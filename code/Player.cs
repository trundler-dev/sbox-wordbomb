using Sandbox;
using WordBomb.Logic;

namespace WordBomb
{
	public partial class Player : Entity
	{
		[Net] public int LifeCount { get; set; } = 3;
		[Net] public bool IsPlaying { get; set; } = false;
		[Net] public bool IsTurn { get; set; } = false;
		[Net] public bool SubmittedWord { get; set; } = false;
		[Net] public string LastSubmittedWord { get; set; } = "";
		[Net] public string Prompt { get; set; } = "";

		[Net] public TimeSince TimeSince { get; set; }

		public Player()
		{
			Transmit = TransmitType.Always;
		}

		public void StartTurn()
		{
			var game = Game.Instance;

			if (game.TurnsWithoutLoss > 10)
			{
				Prompt = PromptDictionary.GetRandomThreeLetterPrompt();
				TimeSince = 2;
			}
			else
			{
				Prompt = PromptDictionary.GetRandomTwoLetterPrompt();
				TimeSince = 0;
			}

			SubmittedWord = false;
			IsTurn = true;
			TimeSince = 0;
		}

		public void FinishTurn()
		{
			if ( Host.IsServer )
			{
				IsTurn = false;

				// If word is not submitted, remove a life. If lives reaches 0, user is no longer playing.
				if ( !SubmittedWord ) LifeCount--;
				if ( LifeCount <= 0 )
				{
					IsPlaying = false;
					Game.Instance.LivingPlayers--;
				}
			}
		}
	}
}
