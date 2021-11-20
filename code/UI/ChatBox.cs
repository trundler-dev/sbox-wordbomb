using Sandbox;
using Sandbox.UI;
using WordBomb.Helpers;

namespace WordBomb.UI
{
	public partial class ChatEntry : Panel
	{
		public Label NameLabel;
		public Label MessageLabel;

		public ChatEntry()
		{
			NameLabel = AddChild<Label>( "ChatNameLabel" );
			MessageLabel = AddChild<Label>( "ChatMessageLabel" );
		}
	}

	public partial class ChatBox : Panel
	{
		public static ChatBox Instance;
		public Panel ChatWrapper;
		public TextEntry ChatInput;
		public Label Title;

		public ChatBox()
		{
			Instance = this;

			Title = AddChild<Label>( "Title" );
			Title.Text = "Chat";

			ChatWrapper = AddChild<Panel>( "ChatWrapper" );
			ChatWrapper.PreferScrollToBottom = true;

			ChatInput = AddChild<TextEntry>( "ChatInput" );
			ChatInput.Placeholder = Consts.CHAT_INPUT_HELP_MESSAGE;
			ChatInput.AddEventListener( "onsubmit", () => Submit() );
			ChatInput.AddEventListener( "onblur", () => CloseChat() );
		}

		public override void Tick()
		{
			if ( Input.Pressed( InputButton.Score ) )
			{
				OpenChat();
			}
			base.Tick();
		}

		private void OpenChat()
		{
			ChatInput.Placeholder = "";
			ChatInput.Focus();
		}

		private void CloseChat()
		{
			ChatInput.Placeholder = Consts.CHAT_INPUT_HELP_MESSAGE;
			ChatInput.Blur();
		}

		private void Submit()
		{
			CloseChat();

			string message = ChatInput.Text.Trim();
			ChatInput.Text = "";

			if ( string.IsNullOrWhiteSpace( message ) ) return;

			Say( message );
		}

		public void AddEntry( string name, string message )
		{
			ChatEntry entry;
			entry = ChatWrapper.AddChild<ChatEntry>();
			entry.NameLabel.Text = name;
			entry.MessageLabel.Text = message;
		}

		[ServerCmd( "say" )]
		public static void Say( string message )
		{
			Assert.NotNull( ConsoleSystem.Caller );

			if ( message.Contains( '\n' ) || message.Contains( '\r' ) )
				return;

			Log.Info( $"{ConsoleSystem.Caller}: {message}" );
			CreateChatEntry( To.Everyone, ConsoleSystem.Caller.Name, $"avatar:{ConsoleSystem.Caller.PlayerId}", message );
		}

		[ClientCmd( "chat_add", CanBeCalledFromServer = true )]
		public static void CreateChatEntry( string name, string avatar, string message )
		{
			Instance?.AddEntry( name, message );
		}

	}
}
