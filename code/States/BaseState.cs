using Sandbox;
using System.Collections.Generic;

namespace WordBomb.States
{
	public abstract partial class BaseState : BaseNetworkable
	{
		public virtual int StateDuration => 0;
		public virtual string StateName => "";
		public virtual bool ShowTimer => false;

		public List<Player> PlayerList = new();

		public float StateEndTime { get; set; }

		public float TimeLeft
		{
			get
			{
				return StateEndTime - Time.Now;
			}
		}

		[Net] public int TimeLeftSeconds { get; set; }

		public void Start()
		{
			if ( Host.IsServer && StateDuration > 0 )
			{
				StateEndTime = Time.Now + StateDuration;
			}

			OnStart();
		}

		public void Finish()
		{
			if ( Host.IsServer )
			{
				StateEndTime = 0f;
				PlayerList.Clear();
			}

			OnFinish();
		}

		public void AddPlayer( Player player )
		{
			Host.AssertServer();

			if ( !PlayerList.Contains( player ) )
				PlayerList.Add( player );
		}

		public virtual void OnPlayerJoin( Player player ) { }

		public virtual void OnPlayerLeave( Player player ) { }

		public virtual void OnTick() { }

		public virtual void OnSecond()
		{
			if ( Host.IsServer )
			{
				if ( StateEndTime > 0 && Time.Now >= StateEndTime )
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

		protected virtual void OnStart()
		{
			Log.Info( $"Started {StateName} State" );
		}

		protected virtual void OnFinish() { }

		protected virtual void OnTimeUp() { }


	}
}
