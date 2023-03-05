namespace DigiWar.Player;

partial class PlayerPawn : AnimatedEntity
{
	[ClientInput] public Vector3 InputDirection { get; protected set; }
	[ClientInput] public Angles ViewAngles { get; set; }

	public void MoveToSpawnpoint()
	{
		var spawnpoint = All.OfType<SpawnPoint>().OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

		if ( spawnpoint != null )
		{
			var tx = spawnpoint.Transform;
			tx.Position = tx.Position + Vector3.Up * 150.0f; // raise it up
			Transform = tx;
		}
	}

	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/sbox_props/watermelon/watermelon.vmdl" );

		MoveToSpawnpoint();

		EnableDrawing = false;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = true;
	}

	public override void BuildInput()
	{
		InputDirection = Input.AnalogMove;

		var look = Input.AnalogLook;

		var viewAngles = ViewAngles;
		viewAngles += look;
		ViewAngles = viewAngles.Normal;
	}

	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );

		Rotation = ViewAngles.ToRotation();

		var movement = InputDirection.Normal;

		Velocity = Rotation * movement;

		Velocity *= Input.Down( InputButton.Run ) ? 1000 : 200;

		MoveHelper helper = new MoveHelper( Position, Velocity );
		helper.Trace = helper.Trace.Size( 16 );
		
		if ( helper.TryMove( Time.Delta ) > 0 )
		{
			Position = helper.Position;
		}
	}

	/// <summary>
	/// Called every frame on the client
	/// </summary>
	public override void FrameSimulate( IClient cl )
	{
		base.FrameSimulate( cl );

		Rotation = ViewAngles.ToRotation();

		Camera.Position = Position;
		Camera.Rotation = Rotation;

		Camera.FieldOfView = Screen.CreateVerticalFieldOfView( Game.Preferences.FieldOfView );
		Camera.FirstPersonViewer = this;
	}
}
