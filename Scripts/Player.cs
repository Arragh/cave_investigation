using Godot;

public partial class Player : CharacterBody2D
{
	private string _currentAnimation = string.Empty;

	[Export]
	public AnimatedSprite2D Animation { get; set; }

	[Export]
	public Camera2D Camera { get; set; }

	[Export]
	public int Speed = 400;

	[Export]
	public int Gravity = 2000;

	[Export]
	public int JumpForce = 900;

	public override void _Ready()
	{

	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Движение влево/вправо
		float direction = Input.GetActionStrength("right") - Input.GetActionStrength("left");
		velocity.X = direction * Speed;

		if (this.IsOnFloor())
		{
			if (velocity.Y > 0)
			{
				velocity.Y = 0;
			}

			// Прыжок
			if (Input.IsActionJustPressed("jump"))
			{
				velocity.Y = -JumpForce;
				PlayAnimation("jump");
			}
			else if (velocity.X != 0)
			{
				Animation.FlipH = velocity.X < 0;
				PlayAnimation("run");
			}
			else
			{
				PlayAnimation("idle");
			}
		}
		else
		{
			velocity.Y += Gravity * (float)delta;
			PlayAnimation("jump");
		}

		// Записываем обновлённую скорость
		this.Velocity = velocity;

		// Двигаем тело (без этого коллизий не будет!)
		this.MoveAndSlide();
	}

	private void PlayAnimation(string animation)
	{
		if (_currentAnimation == animation)
		{
			return;
		}

		Animation.Play(animation);
	}
}