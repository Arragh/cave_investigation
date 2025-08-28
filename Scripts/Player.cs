using Cave_investigation.Enums;
using Godot;

public partial class Player : CharacterBody2D
{
	private Vector2 _velocity = Vector2.Zero;
	private PlayerState _currentState = PlayerState.Idle;
	private PlayerState _lastState;
	private float _direction = 1;

	[Export]
	public AnimatedSprite2D AnimatedSprite2D { get; set; }

	[Export]
	public Camera2D Camera2D { get; set; }

	[Export]
	public int Speed = 400;

	[Export]
	public int Gravity = 2000;

	[Export]
	public int JumpForce = 900;

	public override void _Ready()
	{
		AnimatedSprite2D.AnimationFinished += OnAnimationFinished;
	}

	public override void _Process(double delta)
	{
		FlipHorizontally();
		PlayAnimation();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_currentState == PlayerState.Attack)
		{
			return;
		}

		// Движение влево/вправо
		_direction = Input.GetActionStrength("right") - Input.GetActionStrength("left");
		_velocity.X = _direction * Speed;

		if (this.IsOnFloor())
		{
			if (_velocity.Y > 0)
			{
				_velocity.Y = 0;
			}

			// Прыжок
			if (Input.IsActionJustPressed("jump"))
			{
				_velocity.Y = -JumpForce;
				_currentState = PlayerState.Jump;
			}
			else if (Input.IsActionJustPressed("attack"))
			{
				_velocity.X = 0;
				_currentState = PlayerState.Attack;
			}
			else if (_velocity.X != 0)
			{
				_currentState = PlayerState.Run;
			}
			else
			{
				_currentState = PlayerState.Idle;
			}
		}
		else
		{
			_velocity.Y += Gravity * (float)delta;
		}

		// Записываем обновлённую скорость
		this.Velocity = _velocity;

		// Двигаем тело (без этого коллизий не будет!)
		this.MoveAndSlide();
	}

	private void FlipHorizontally()
	{
		if (_direction == 0)
		{
			return;
		}

		if (_velocity.X < 0 && _direction == 1)
			{
				_direction = -1;
			}
			else if (_velocity.X > 0 && _direction == -1)
			{
				_direction = 1;
			}

		AnimatedSprite2D.FlipH = _direction == -1;
	}

	private void PlayAnimation()
	{
		if (_lastState == _currentState)
		{
			return;
		}

		switch (_currentState)
		{
			case PlayerState.Idle:
				AnimatedSprite2D.Play("idle");
				break;

			case PlayerState.Run:
				AnimatedSprite2D.Play("run");
				break;

			case PlayerState.Jump:
				AnimatedSprite2D.Play("jump");
				break;

			case PlayerState.Attack:
				AnimatedSprite2D.Play("attack");
				break;

			case PlayerState.Dead:
				AnimatedSprite2D.Play("dead");
				break;
		}

		_lastState = _currentState;
	}

	private void OnAnimationFinished()
	{
		if (_currentState == PlayerState.Attack)
		{
			_currentState = PlayerState.Idle;
		}
	}
}