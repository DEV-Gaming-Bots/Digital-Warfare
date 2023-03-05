using System.Linq;

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
			tx.Position = tx.Position + Vector3.Up * 150.0f;
			Transform = tx;
		}
	}

	public override void Spawn()
	{
		base.Spawn();

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
		viewAngles.pitch = viewAngles.pitch.Clamp( -90, 90 );
		ViewAngles = viewAngles.Normal;
	}

	Vector3 lookAt;

	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );

		Rotation = ViewAngles.ToRotation();

		float speed = 350.0f;
		float deltaSpeed = 8.0f;

		var velocity = Vector3.Zero;

		if ( Input.Down( InputButton.Forward ) )
			velocity += Camera.Rotation.Forward.WithZ( 0 ) * speed;

		if ( Input.Down( InputButton.Back ) )
			velocity += Camera.Rotation.Backward.WithZ( 0 ) * speed;

		if ( Input.Down( InputButton.Left ) )
			velocity += Camera.Rotation.Left * speed;

		if ( Input.Down( InputButton.Right ) )
			velocity += Camera.Rotation.Right * speed;

		if(Input.MouseWheel != 0)
		{
			velocity = velocity.WithZ( Input.MouseWheel * 250 );
		}

		var lookAtPosition = lookAt + velocity * Time.Delta;
		lookAt = lookAtPosition;

		var helper = new MoveHelper( Camera.Position, velocity );
		helper.Trace = helper.Trace.Size( 16 );

		if(helper.TryMoveWithStep(Time.Delta * deltaSpeed, 2.0f) > 0)
		{
			lookAt = helper.Position;
		}

		Camera.Position = Camera.Position.LerpTo( lookAt, Time.Delta * deltaSpeed / 2 );	
	}

	/// <summary>
	/// Called every frame on the client
	/// </summary>
	public override void FrameSimulate( IClient cl )
	{
		base.FrameSimulate( cl );

		//Camera.Position = Position;
		Camera.Rotation = Rotation;

		Camera.FieldOfView = Screen.CreateVerticalFieldOfView( Game.Preferences.FieldOfView );
		Camera.FirstPersonViewer = this;
	}
}
