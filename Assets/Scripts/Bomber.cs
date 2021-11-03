using UnityEngine;

public class Bomber : Gun, IOrderPlaceable
{
    private int _ordersAmount; 
    private int _ticksGone = 0;
    
    public Bomber(Placer placer, Vector2Int position, BlockInfo info) : base(placer, position, info) {}

    public void PlaceOrder()
    {
        _ordersAmount += 1;
        Debug.Log(_ordersAmount + " orders placed");
    }
    
    protected override void Fire()
    {
        new Bomb(_placer, _position + new Vector2Int((_placer.First() ? -1 : 1), 0), _info.bulletInfo);
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