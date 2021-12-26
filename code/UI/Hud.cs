using Sandbox;
using Sandbox.UI;

namespace WordBomb.UI
{
	public partial class Hud : HudEntity<RootPanel>
	{
		public Hud()
		{
			if ( IsClient )
			{
				RootPanel.StyleSheet.Load( "/UI/Stylesheets/Hud.scss" );
			}
		}
	}
}
