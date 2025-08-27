using Godot;
using System;

public partial class Skeleton : CharacterBody2D
{
	[Export]
	public AnimatedSprite2D IdleAnimation { get; set; }

	[Export]
	public AnimatedSprite2D WalkAnimation { get; set; }

	[Export]
	public AnimatedSprite2D AttackAnimation { get; set; }

	[Export]
	public AnimatedSprite2D HitAnimation { get; set; }

	[Export]
	public AnimatedSprite2D DeathAnimation { get; set; }

	public override void _PhysicsProcess(double delta)
	{

	}
}
