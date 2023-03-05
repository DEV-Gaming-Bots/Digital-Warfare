global using Sandbox.UI.Construct;
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
	public static DigiWarGame Instance => Current as DigiWarGame;

	public DigiWarGame()
	{

	}

	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );

		var pawn = new PlayerPawn();
		client.Pawn = pawn;
	}
}
