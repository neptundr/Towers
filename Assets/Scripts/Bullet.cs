using UnityEngine;

public class Bullet : Entity
{
    // private Tower _originTower;
    protected float _damage;

    public Bullet(Placer placer, Vector2Int position, BulletInfo info) : base(placer, position, info)
    {
        _damage = info.damage;
        // _originTower = _placer;
        
        _placer.Timer.SummonBullets += Move;
    }

    protected bool CheckForDetonate()
    {
        return !(Tower.Map[_position.x, _position.y] is null);
    }
    
    protected override void AdditionalDisbandActions()
    {
        _placer.Timer.SummonBullets -= Move;
    }

    /// Use 'base' after 'this'
    protected virtual void Move()
    {
        if (_position.x < 0 || _position.x >= Tower.MapSize.x || _position.y < 0 || _position.y >= Tower.MapSize.y)
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