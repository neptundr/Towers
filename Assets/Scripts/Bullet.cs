using UnityEngine;

public class Bullet : Entity
{
    // private Tower _originTower;
    private float _damage;

    protected bool _movingRight;

    public Bullet(Placer placer, Vector2Int position, BulletInfo info, bool movingRight) : base(placer, position, info)
    {
        _movingRight = movingRight;
        _damage = info.damage;
        // _originTower = _placer;
        
        _placer.Timer.SummonBullets += Move;
    }

    protected bool CheckForDetonate()
    {
        return !(Tower.Map[_position.x, _position.y] is null);
    }
    
    protected override void AdditionDisbandActions()
    {
        _placer.Timer.SummonBullets -= Move;
    }

    /// Use 'base' after 'this'
    protected virtual void Move()
    {
        if (_position.x < 0 || _position.x >= Tower.MapSize.x)
        {
            Disband();
        }
        else
        {   
            if (CheckForDetonate()) Detonate(Tower.Map[_position.x, _position.y]);

            UpdatePicturePosition();
        }
    }

    protected virtual void Detonate(Block block)
    {
        block.Damage(_damage);
        Disband();
    }
}