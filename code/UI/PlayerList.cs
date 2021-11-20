using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;
using System.Linq;

namespace WordBomb.UI
{
	public class PlayerListEntry : Panel
	{
		public Label NameLabel;
		public Image Avatar;

		public PlayerListEntry()
		{
			Avatar = AddChild<Image>();
			NameLabel = AddChild<Label>( "PlayerListNameLabel" );
		}
	}

	public partial class PlayerList : Panel
	{
		public Dictionary<Client, PlayerListEntry> activeEntries = new Dictionary<Client, PlayerListEntry>();
		public Panel scrollablePanel;
		public Label title;

		public PlayerList()
		{
			StyleSheet.Load( "/UI/Stylesheets/PlayerList.scss" );
			title = AddChild<Label>( "Title" );
			title.Text = "Players";
			scrollablePanel = AddChild<Panel>( "Scrollable" );
		}

		public override void Tick()
		{
			base.Tick();

			var deleteList = new List<Client>();
			deleteList.AddRange( activeEntries.Keys );

			foreach ( var client in Entity.All.OfType<Client>() )
			{
				deleteList.Remove( client );

				if ( !activeEntries.TryGetValue( client, out var entry ) )
				{
					entry = CreatePlayerListEntry( client );
					activeEntries[client] = entry;
				}
			}
		}

		public PlayerListEntry CreatePlayerListEntry( Client client )
		{
			var entry = new PlayerListEntry()
			{
				Parent = scrollablePanel
			};
			entry.NameLabel.Text = client.Name;
			entry.Avatar.SetTexture($"avatar:{client.PlayerId}");
			return entry;
		}
	}
}
