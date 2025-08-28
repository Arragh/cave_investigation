using Godot;

public partial class Skeleton : CharacterBody2D
{
	private Vector2 _velocity = Vector2.Zero;
	private float _direction = 1;

	[Export]
	public AnimatedSprite2D AnimatedSprite2D { get; set; }

	[Export]
	public int Speed = 100;

	[Export]
	public int Gravity = 2000;

	public override void _Ready()
	{
		AnimatedSprite2D.Play("walk");
	}

	public override void _PhysicsProcess(double delta)
	{
		_velocity.X = _direction * Speed;
		_velocity.Y += Gravity * (float)delta;

		Velocity = _velocity;
		MoveAndSlide();
	}
}