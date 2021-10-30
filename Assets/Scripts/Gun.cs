using UnityEngine;

public class Gun : Block
{
    protected new readonly GunInfo _info;

    public Gun(Placer placer, Vector2Int position, BlockInfo info) : base(placer, position, info)
    {
        _info = info as GunInfo;
    }
    
    protected virtual void Fire(){}
}