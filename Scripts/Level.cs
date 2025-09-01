using Godot;

public partial class Level : Node
{
	[Export]
	public PackedScene PlayerScene { get; set; }

	[Export]
	public Marker2D StartSpawnPoint { get; set; }

	public override void _Ready()
	{
		var player = PlayerScene.Instantiate<Player>();
		player.Position = StartSpawnPoint.Position;

		AddChild(player);
	}

	public override void _Process(double delta)
	{
	}
}
