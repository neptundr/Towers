using UnityEngine;

public class ResourceAdder : Block
{
    private float _resourcesToAddCount = 0.5f;
    private float _divideCoef = 2;
    private int _ticksGone;
    
    public ResourceAdder(Placer placer, Vector2Int position, BlockInfo info) : base(placer, position, info){}
    
    protected override void DoSth()
    {
        _ticksGone += 1;
        if (_ticksGone >= _info.ticksToProduce)
        {
            _ticksGone = 0;
            _placer.AddResources(_resourcesToAddCount / (_placer.GetResources() < 0 ? _divideCoef : 1));
        }
    }
}