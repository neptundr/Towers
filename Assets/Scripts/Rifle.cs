using UnityEngine;

public class Rifle : Gun, IOrderPlaceable
{
    private bool _ordered;
    private int _ticksGone = 0;

    public Rifle(Placer placer, Vector2Int position, GunInfo info) : base(placer, position, info) {}

    public void PlaceOrder()
    {
        _ordered = true;
    }

    protected override void Fire()
    {
        new RifleBullet(_placer, _position, _info.bulletInfo,
            (_info.shootingRight ? 1 : -1) * (_placer.First() ? 1 : -1) > 0);
    }

    protected override void DoSth()
    {
        if (_ordered)
        {
            _ticksGone += 1;

            if (_ticksGone >= _info.ticksToProduce)
            {
                Fire();
                _ticksGone = 0;
                _ordered = false;
            }
        }
    }
}
