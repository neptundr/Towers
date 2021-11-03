using UnityEngine;

public class Rifle : Gun, IOrderPlaceable
{
    private int _ordersAmount; 
    private int _ticksGone = 0;

    public Rifle(Placer placer, Vector2Int position, GunInfo info) : base(placer, position, info) {}

    public void PlaceOrder()
    {
        _ordersAmount += 1;
        Debug.Log(_ordersAmount + " orders placed");
    }

    protected override void Fire()
    {
        new RifleBullet(_placer, _position, _info.bulletInfo);
    }

    protected override void DoSth()
    {
        if (_ordersAmount > 0)
        {
            _ticksGone += 1;

            if (_ticksGone >= _info.ticksToProduce && _placer.RemoveResources(_info.bulletInfo.resource))
            {
                Fire();
                _ticksGone = 0;
                _ordersAmount -= 1;
            }
        }
    }
}
