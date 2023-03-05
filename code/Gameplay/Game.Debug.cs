using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiWar;

public partial class DigiWarGame
{
	public static DigiWarGame Instance => Current as DigiWarGame;

	[ConVar.Replicated("dw.debug")]
	public static bool Debugging { get; set; }

	[ConCmd.Server("dw.player.reset")]
	public static void ResetPlayer()
	{
		if ( !Debugging ) return;

		if ( ConsoleSystem.Caller.Pawn is null 
			|| ConsoleSystem.Caller.Pawn is not PlayerPawn player ) 
			return;

		player.Spawn();
	}
}
