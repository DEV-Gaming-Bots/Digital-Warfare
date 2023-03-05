namespace DigiWar.UI;

public class HudBase : RootPanel
{
	public static HudBase Current;

	public HudBase()
	{
		Current?.Delete();
		Current = null;

		AddChild<ChatBox>();
		AddChild<Scoreboard<ScoreboardEntry>>();

		Current = this;
	}
}
