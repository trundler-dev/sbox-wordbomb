using Sandbox.UI;

namespace WordBomb.UI
{
	public partial class SideBar : Panel
	{
		public SideBar()
		{
			StyleSheet.Load( "/UI/Stylesheets/SideBar.scss" );
			AddChild<PlayerList>();
			AddChild<ChatBox>();
		}
	}
}
