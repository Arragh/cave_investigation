using Godot;

public partial class Skeleton : CharacterBody2D
{
	[Export]
	public AnimatedSprite2D Animation { get; set; }


	public override void _PhysicsProcess(double delta)
	{

	}
}