using UnityEngine;

public class ResourceAdder : Block
{
    [SerializeField] private uint _resourcesToAddCount; 
    
    public ResourceAdder(Placer placer, Vector2Int position, BlockInfo info) : base(placer, position, info){}
    
    protected override void DoSth()
    {
        // .AddResources((int) _resourcesToAddCount);
    }
}