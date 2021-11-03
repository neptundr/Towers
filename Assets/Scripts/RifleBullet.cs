using UnityEngine;

public class RifleBullet : Bullet
{
    public RifleBullet(Placer placer, Vector2Int position, BulletInfo info) : base(placer, position, info) {}

    protected override void Move()
    {
        _position = new Vector2Int(_position.x + (_placer.First() ? 1 : -1), _position.y);
        base.Move();
    }
}