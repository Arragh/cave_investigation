using Cave_investigation.Enums;
using Godot;

namespace Cave_investigation.Abstracts;

public abstract partial class Enemy : CharacterBody2D
{
    private float _direction = -1;
    private RayCast2D _currentRayCast2D => _direction == -1 ? RayCastLeft : RayCastRight;
    private EnemyState _currentState = EnemyState.Walk;
    private EnemyState _lastState = EnemyState.Idle;
	private Player _player = null;

    [Export]
    public int Speed = 100;

    [Export]
    public int Gravity = 2000;

    [Export]
    public AnimatedSprite2D AnimatedSprite2D { get; set; }

    [Export]
    public RayCast2D RayCastLeft { get; set; }

    [Export]
    public RayCast2D RayCastRight { get; set; }

    [Export]
    public Area2D AttackArea { get; set; }
    
    public override void _Ready()
	{
		AnimatedSprite2D.AnimationFinished += OnAnimationFinished;

		AttackArea.BodyEntered += OnAttackAreaBodyEntered;
		AttackArea.BodyExited += OnAttackAreaBodyExited;

	}

	public override void _Process(double delta)
	{
		PlayAnimation();
		UpdateFlipH();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_currentState == EnemyState.Attack)
		{
			Velocity = Vector2.Zero;
			MoveAndSlide();

			return;
		}

		Velocity = new Vector2(_direction * Speed, Velocity.Y);

		if (!IsOnFloor())
		{
			Velocity += Vector2.Down * Gravity * (float)delta;
		}

		if (Velocity.X != 0)
		{
			_currentState = EnemyState.Walk;
		}

		MoveAndSlide();
		CheckEdge();
	}

	private void UpdateFlipH()
	{
		if (_currentState == EnemyState.Attack && _player != null)
		{
			AnimatedSprite2D.FlipH = _player.GlobalPosition.X < GlobalPosition.X;
		}
		else
		{
			AnimatedSprite2D.FlipH = _direction == -1;
		}
	}

	private void CheckEdge()
	{
		if (!_currentRayCast2D.IsColliding())
		{
			_direction *= -1;
		}
	}

	private void OnAttackAreaBodyEntered(Node body)
	{
		if (body is Player player)
		{
			_player = player;
		}

		if (_player != null && _currentState != EnemyState.Attack)
		{
			_currentState = EnemyState.Attack;
		}
	}

	private void OnAttackAreaBodyExited(Node body)
	{
		if (body is Player)
		{
			_player = null;
			_currentState = EnemyState.Idle;
		}
	}

	private void OnAnimationFinished()
	{
		if (_currentState == EnemyState.Attack)
		{
			_currentState = EnemyState.Idle;
		}
	}

	private void PlayAnimation()
	{
		if (_lastState == _currentState)
		{
			return;
		}

		switch (_currentState)
		{
			case EnemyState.Idle:
				AnimatedSprite2D.Play("idle");
				break;

			case EnemyState.Walk:
				AnimatedSprite2D.Play("walk");
				break;

			case EnemyState.Attack:
				AnimatedSprite2D.Play("attack");
				break;

			case EnemyState.Hit:
				AnimatedSprite2D.Play("hit");
				break;

			case EnemyState.Dead:
				AnimatedSprite2D.Play("dead");
				break;
		}

		_lastState = _currentState;
	}
}