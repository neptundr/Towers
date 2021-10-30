using UnityEngine;

public class RifleBullet : Bullet
{
    public RifleBullet(Placer placer, Vector2Int position, BulletInfo info, bool movingRight) : base(placer, position, info, movingRight) {}

    protected override void Move()
    {
        _position = new Vector2Int(_position.x + (_movingRight ? 1 : -1), _position.y);
        base.Move();
    }
}