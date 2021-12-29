using Sandbox;

namespace WordBomb.States
{
	public abstract partial class State : BaseNetworkable
	{
		public virtual string StateName => string.Empty;

		public virtual float StateDuration => 0;
		public float StateEndTime { get; set; }
		public float TimeLeft { get { return StateEndTime - Time.Now; } }
		[Net] public int TimeLeftSeconds { get; set; }

		private RealTimeUntil NextSecond;

		public void Start()
		{
			if ( Host.IsServer && StateDuration > 0 )
			{
				StateEndTime = Time.Now + StateDuration;
				NextSecond = 1f;
			}

			OnStart();
		}

		public void Finish()
		{
			if ( Host.IsServer )
			{
				StateEndTime = 0f;
			}

			OnFinish();
		}

		public virtual void OnPlayerJoin( Player player ) { }

		public virtual void OnPlayerLeave( Player player ) { }

		public virtual void OnTick()
		{
			if ( NextSecond )
			{
				OnSecond();
				NextSecond = 1f;
			}
		}

		public virtual void OnSecond()
		{
			if ( Host.IsServer )
			{
				if ( StateEndTime > 0f && Time.Now >= StateEndTime )
				{
					StateEndTime = 0f;
					OnTimeUp();
				}
				else
				{
					TimeLeftSeconds = TimeLeft.CeilToInt();
				}
			}
		}

		protected virtual void OnStart() { }

		protected virtual void OnFinish() { }

		protected virtual void OnTimeUp() { }

	}
}
