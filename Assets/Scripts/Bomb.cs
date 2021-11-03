using UnityEngine;

public class Bomb : Bullet
{
    private static int _squareSide = 3;
    public Bomb(Placer placer, Vector2Int position, BulletInfo info) : base(placer, position, info) {}

    protected override void Move()
    {
        _position = new Vector2Int(_position.x, _position.y - 1);
        base.Move();
    }
    
    protected override void Detonate(Block block)
    {
        Tower.SetShouldFall(false);
        for (int x = -1; x < _squareSide - 1; x++)
        {
            for (int y = 0; y < _squareSide; y++)
            {
                if (_position.x + x < Tower.MapSize.x && _position.x + x < Tower.MapSize.x)
                {
                    if (!(Tower.Map[_position.x + x, _position.y + y] is null))
                    {
                        Tower.Map[_position.x + x, _position.y + y].Damage(_damage);
                    }
                }
            }
        }
        Tower.SetShouldFall(true);
        Tower.SomethingChanged();
        Disband();
    }
}