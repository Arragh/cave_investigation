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
		if (_currentState == PlayerState.Dead)
		{
			return;
		}

		if (_currentState == PlayerState.Jump)
		{
			Jump();
		}

		if (_currentState == PlayerState.Attack)
		{
			AttackEnemy();
		}

		if (_currentState != PlayerState.Attack)
		{
			_direction = Input.GetActionStrength("action_right") - Input.GetActionStrength("action_left");
			_velocity.X = _direction * Speed;

			if (this.IsOnFloor())
			{
				if (Input.IsActionJustPressed("action_jump") && _currentState != PlayerState.Jump)
				{
					_currentState = PlayerState.Jump;
				}
				else if (Input.IsActionJustPressed("action_attack") && _currentState != PlayerState.Attack)
				{
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
		}

		Velocity = _velocity;
		MoveAndSlide();
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

	public void Die()
	{
		if (_currentState != PlayerState.Dead)
		{
			_currentState = PlayerState.Dead;
			CollisionShape2D.Disabled = true;
			GD.Print("Player is dead!");
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
			_damageApplied = false;
		}

		if (_currentState == PlayerState.Jump)
		{
			_velocity.Y = 0;
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
		_velocity.X = 0;

		if (_currentState == PlayerState.Attack && _enemy != null)
		{
			int frameCount = AnimatedSprite2D.SpriteFrames.GetFrameCount("attack");

			if (AnimatedSprite2D.Frame >= 2 && !_damageApplied)
			{
				_enemy.TakeDamage(WeaponDamage);
				_damageApplied = true;
			}
		}
	}

	private void Jump()
	{
		_velocity.Y = -JumpForce;
	}
}