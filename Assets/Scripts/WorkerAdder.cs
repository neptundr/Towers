using UnityEngine;

public class WorkerAdder : Block, IOrderPlaceable
{
    private int _ticksGone;
    private int _order;

    public WorkerAdder(Placer placer, Vector2Int position, BlockInfo info) : base(placer, position, info){}
    
    public void PlaceOrder()
    {
        _order += 1;
        Debug.Log("Order placed " + _order);
    }
    
    protected override void DoSth()
    {
        if (_order > 0)
        {
            _ticksGone += 1;

            if (_ticksGone >= _info.ticksToProduce)
            {
                _ticksGone = 0;
                Tower.AddWorker(_position, _placer.First());
                _order -= 1;
                Debug.Log("Order complete");
            }
        }
    }
}