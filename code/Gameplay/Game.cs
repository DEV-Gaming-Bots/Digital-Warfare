global using System;
global using System.IO;
global using System.Linq;
global using System.Threading.Tasks;
global using Sandbox;
global using Sandbox.UI;
global using Sandbox.UI.Construct;
global using DigiWar.Player;
global using DigiWar.UI;

namespace DigiWar;

public partial class DigiWarGame : GameManager
{
	public DigiWarGame()
	{
		Debugging = Game.IsEditor;

		if(Game.IsServer)
		{

		}

		if(Game.IsClient)
		{
			_ = new HudBase();
		}
	}

	[Event.Hotload]
	protected void HotloadGame()
	{
		if ( Game.IsServer )
		{

		}

		if ( Game.IsClient )
		{
			_ = new HudBase();
		}
	}

	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );

		var pawn = new PlayerPawn();
		client.Pawn = pawn;
	}
}
