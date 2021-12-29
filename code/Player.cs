using Sandbox;

namespace WordBomb
{
	public partial class Player : Entity
	{
		[Net] public int LifeCount { get; set; } = 3;

		[Net] public bool IsPlaying { get; set; } = false;
		[Net] public bool IsTurn { get; set; } = false;
		[Net] public bool HasSubmitted { get; set; } = false;

		[Net] public string TypedWord { get; set; } = "";
		[Net] public string Prompt { get; set; } = "";

		public Player()
		{
			Transmit = TransmitType.Always;
		}
	}
}
