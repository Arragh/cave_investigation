using Godot;

public partial class MainMenu : Control
{
	[Export]
	public Button StartNewGameButton { get; set; }

	[Export]
	public Button ExitButton { get; set; }

	public override void _Ready()
	{
		StartNewGameButton.Pressed += OnNewGameButtonPressed;
		ExitButton.Pressed += OnExitButtonPressed;
	}

	public override void _Process(double delta)
	{
	}

	private void OnNewGameButtonPressed()
	{
		var level = GD.Load<PackedScene>("res://Scenes/level.tscn");
		GetTree().ChangeSceneToPacked(level);
	}

	private void OnExitButtonPressed()
	{
		GetTree().Quit();
	}
}
