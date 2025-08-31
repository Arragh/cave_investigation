using Cave_investigation.Abstracts;
using Godot;

public partial class Skeleton : Enemy
{
    [Export]
    public override int Damage { get; set; } = 4;

    [Export]
    public override int MaxHealth { get; set; } = 8;

    public override void AttackPlayer()
    {
        Velocity = Vector2.Zero;
        int frameCount = AnimatedSprite2D.SpriteFrames.GetFrameCount("attack");

        if ((AnimatedSprite2D.Frame == 4 || AnimatedSprite2D.Frame == 8) && !_damageApplied)
        {
            _player.TakeDamage(Damage);
            _damageApplied = true;
        }

        if (AnimatedSprite2D.Frame == 5 || AnimatedSprite2D.Frame == frameCount - 1)
        {
            _damageApplied = false;
        }
    }
}