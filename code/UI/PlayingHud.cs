using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;
using System;

namespace WordBomb.UI
{
	public partial class PlayingHud : Panel
	{
		public PlayingHud Instance;

		public Label turnLabel;
		public Panel entryPanel;
		public Label promptLabel;
		public Label timerLabel;
		public TextEntry submissionTextEntry;

		public Dictionary<Player, PlayerEntry> entries = new();

		public PlayingHud()
		{
			Instance = this;

			StyleSheet.Load( "/UI/Stylesheets/PlayingHud.scss" );
			turnLabel = AddChild<Label>( "TurnLabel" );
			entryPanel = AddChild<Panel>( "Entries" );
			promptLabel = AddChild<Label>( "PromptLabel" );
			timerLabel = entryPanel.AddChild<Label>( "TimerLabel" );
			submissionTextEntry = AddChild<SubmissionTextEntry>();

			foreach ( var client in Client.All )
			{
				var player = client.Pawn as Player;
				PlayerEntry entry = entryPanel.AddChild<PlayerEntry>();
				entry.Avatar.SetTexture( $"avatar:{client.PlayerId}" );
				entry.NameLabel.Text = client.Name;
				entry.UpdateLifeCount(player.LifeCount);
				entry.Style.Position = PositionMode.Absolute;

				entries.Add( player, entry );
			}
		}
		
		public override void Tick()
		{
			var game = Game.Instance;
			if ( game == null ) return;

			var state = game.State;
			if ( state == null ) return;

			var player = game.ActivePlayer;

			turnLabel.Text = $"{player.Client.Name}'s Turn";
			promptLabel.Text = $"{player.Client.Name}, write an English word that contains {player.Prompt}.";

			RefreshActivePlayers();
			CalculateScreenPositionForPlayers();

			foreach ( var entry in entries )
			{
				entry.Value.UpdateTypeLabel( entry.Key.LastSubmittedWord );
				entry.Value.TypeLabel.SetClass( "hide", entry.Key.LastSubmittedWord == "" );
			}

			timerLabel.Text = (5 - (int)player.TimeSince).ToString();
			timerLabel.Style.Top = (int)((entryPanel.Box.Rect.width / 2) - (timerLabel.Box.Rect.width / 2)) * ScaleFromScreen;
			timerLabel.Style.Left = (int)((entryPanel.Box.Rect.height / 2) - (timerLabel.Box.Rect.width / 2)) * ScaleFromScreen;

			base.Tick();
		}

		private void RefreshActivePlayers() {
			var deleteKeys = new List<Player>();
			foreach ( var entry in entries )
			{
				var player = entry.Key;
				if ( !player.IsPlaying )
				{
					entry.Value.Delete();
					deleteKeys.Add( player );
				}
			}
			foreach ( var player in deleteKeys )
			{
				entries.Remove( player );
			}

			foreach ( var entry in entries )
			{
				var player = entry.Key;
				entry.Value.SetClass( "active", player.IsTurn );
				entry.Value.UpdateLifeCount( player.LifeCount );
			}
		}

		private void CalculateScreenPositionForPlayers()
		{
			var radius = entryPanel.Box.Rect.width / 2;

			int i = 0;

			foreach ( var entryPair in entries )
			{
				var entry = entryPair.Value;

				var x0 = ((entryPanel.Box.Rect.width / 2) - (entry.Box.Rect.width / 2)) * ScaleFromScreen;
				var y0 = ((entryPanel.Box.Rect.height / 2) - (entry.Box.Rect.height / 2)) * ScaleFromScreen;

				entry.Style.Top = (int)(y0 + (radius * Math.Sin( 2 * Math.PI * i / entries.Count ) * ScaleFromScreen));
				entry.Style.Left = (int)(x0 + (radius * Math.Cos( 2 * Math.PI * i / entries.Count ) * ScaleFromScreen));

				i++;
			}
		}

		[ServerCmd]
		public static void RotateTurn()
		{
			var game = Game.Instance;
			if ( game == null ) return;

			game.AssignNextPlayer();
		}
	}

	public partial class PlayerEntry : Panel
	{
		public Panel UserPanel;
		public Image Avatar;
		public Label NameLabel;
		public Label LivesCount;
		public Label TypeLabel;

		public PlayerEntry()
		{
			UserPanel = AddChild<Panel>( "UserPanel" );
			NameLabel = UserPanel.AddChild<Label>();
			Avatar = UserPanel.AddChild<Image>();
			LivesCount = UserPanel.AddChild<Label>();
			TypeLabel = AddChild<Label>( "TypedLabel" );

			UserPanel.Style.Width = Length.Pixels( 100 );
			UserPanel.Style.Height = Length.Pixels( 125 );
		}

		public void UpdateLifeCount( int lifeCount )
		{
			LivesCount.Text = $"{lifeCount} Lives";
		}

		public void UpdateTypeLabel( string newmessage )
		{
			TypeLabel.Text = newmessage;
		}
	}
}
