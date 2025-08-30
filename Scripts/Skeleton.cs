using Cave_investigation.Abstracts;
using Cave_investigation.Enums;
using Godot;

public partial class Skeleton : Enemy
{
    [Export]
    public int Damage = 4;

    [Export]
    public override int MaxHealth { get; set; } = 4;

    public override void AttackPlayer()
    {
        if (_currentState == EnemyState.Attack)
        {
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
}