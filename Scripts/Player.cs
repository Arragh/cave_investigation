using Cave_investigation.Abstracts;
using Cave_investigation.Enums;
using Godot;

public partial class Player : CharacterBody2D
{
	private Vector2 _velocity = Vector2.Zero;
	private PlayerState _currentState = PlayerState.Idle;
	private PlayerState _lastState = PlayerState.Jump;
	private float _direction = 1;
	private float _lastDirection = 1;
	private int _currentHealth = 0;
	private Enemy _enemy = null;
	private bool _damageApplied = false;

	[Export]
	public AnimatedSprite2D AnimatedSprite2D { get; set; }

	[Export]
	public CollisionShape2D CollisionShape2D { get; set; }

	[Export]
	public Camera2D Camera2D { get; set; }

	[Export]
	public ProgressBar ProgressBar { get; set; }

	[Export]
	public Area2D AttackArea { get; set; }

	[Export]
	public int Speed = 400;

	[Export]
	public int Gravity = 2000;

	[Export]
	public int JumpForce = 900;

	[Export]
	public int MaxHealth { get; set; } = 10;

	[Export]
	public int WeaponDamage = 5;

	public override void _Ready()
	{
		AnimatedSprite2D.AnimationFinished += OnAnimationFinished;

		_currentHealth = MaxHealth;
		ProgressBar.MaxValue = MaxHealth;
		ProgressBar.Value = _currentHealth;

		AttackArea.BodyEntered += OnAttackAreaBodyEntered;
		AttackArea.BodyExited += OnAttackAreaBodyExited;
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
			AttackEnemy();
			return;
		}

		if (_currentState == PlayerState.Dead)
		{
			return;
		}

		// Движение влево/вправо
		_direction = Input.GetActionStrength("action_right") - Input.GetActionStrength("action_left");
		_velocity.X = _direction * Speed;

		if (this.IsOnFloor())
		{
			if (_velocity.Y > 0)
			{
				_velocity.Y = 0;
			}

			// Прыжок
			if (Input.IsActionJustPressed("action_jump"))
			{
				_velocity.Y = -JumpForce;
				_currentState = PlayerState.Jump;
			}
			else if (Input.IsActionJustPressed("action_attack"))
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

			if (_velocity.Y != 0)
			{
				_currentState = PlayerState.Jump;
			}
		}

		// Записываем обновлённую скорость
		this.Velocity = _velocity;

		// Двигаем тело (без этого коллизий не будет!)
		this.MoveAndSlide();
	}

	public void TakeDamage(int damage)
	{
		if (_currentHealth > 0)
		{
			_currentHealth -= damage;
			ProgressBar.Value = _currentHealth;

			if (_currentHealth <= 0)
			{
				_currentState = PlayerState.Dead;
				CollisionShape2D.Disabled = true;
				GD.Print("Player is dead!");
			}
		}
	}

	public bool IsDead()
	{
		return _currentState == PlayerState.Dead;
	}

	private void FlipHorizontally()
	{
		if (_direction == 0)
		{
			return;
		}

		if (_direction < 0)
		{
			AttackArea.RotationDegrees = 180;
		}
		else
		{
			AttackArea.RotationDegrees = 0;
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

	private void OnAttackAreaBodyEntered(Node body)
	{
		if (body is Enemy enemy)
		{
			GD.Print("ENEMY SPOTED");
			_enemy = enemy;
		}
	}

	private void OnAttackAreaBodyExited(Node body)
	{
		if (body is Enemy)
		{
			GD.Print("ENEMY LOST");
			_enemy = null;
		}
	}

	private void AttackEnemy()
	{
		if (_currentState == PlayerState.Attack && _enemy != null)
		{
			int frameCount = AnimatedSprite2D.SpriteFrames.GetFrameCount("attack");

			if (AnimatedSprite2D.Frame == 2 && !_damageApplied)
			{
				_enemy.TakeDamage(WeaponDamage);
				_damageApplied = true;
			}

			if (AnimatedSprite2D.Frame == 5 && AnimatedSprite2D.Frame == frameCount - 1)
			{
				_damageApplied = false;
			}
		}
	}
}