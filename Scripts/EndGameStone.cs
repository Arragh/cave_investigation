using Godot;

public partial class EndGameStone : Node2D
{
	[Export]
	public Area2D Area2D { get; set; }

	public override void _Ready()
	{
		Area2D.BodyEntered += OnBodyEntered;
	}

	public override void _Process(double delta)
	{
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			CallDeferred(nameof(QuitToMainMenu));
		}
	}

	private void QuitToMainMenu()
	{
		var mainMenu = GD.Load<PackedScene>("res://Scenes/main_menu.tscn");
		GetTree().ChangeSceneToPacked(mainMenu);
	}
}
