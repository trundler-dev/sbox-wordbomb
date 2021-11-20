using Sandbox;
using System.Linq;

namespace WordBomb.States
{
	public class EndState : BaseState
	{
		public override string StateName => "END";
		public override int StateDuration => 15;

		protected override void OnStart()
		{
			base.OnStart();
		}

		protected override void OnFinish()
		{
			var players = Client.All.Select( ( client ) => client.Pawn as Player );

			foreach ( var player in players )
			{
				player.Delete();
			}

			var clients = Client.All;

			foreach ( var client in clients )
			{
				client.Pawn = new Player();
			}

			Game.Instance.Dict.ClearUsedWordSubmissions();
			Game.Instance.TurnsWithoutLoss = 0;

			base.OnFinish();
		}

		protected override void OnTimeUp()
		{
			Game.Instance.ChangeState( new WaitingState() );

			base.OnTimeUp();
		}

	}
}
