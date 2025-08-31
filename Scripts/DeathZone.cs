using Godot;

public partial class DeathZone : Area2D
{
	private Player _player;

	public CollisionShape2D CollisionShape2D { get; set; }

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}

	public override void _Process(double delta)
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_player != null)
		{
			_player.Die();
		}
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player player)
		{
			_player = player;
		}
	}
}