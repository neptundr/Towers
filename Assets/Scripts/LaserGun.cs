using System;
using UnityEngine;

public class LaserGun : Gun
{
    public event Action move;

    private int _ticksGone;
    
    public LaserGun(Placer placer, Vector2Int position, BlockInfo info) : base(placer, position, info) {}

    protected override void DoSth()
    {
        _ticksGone += 1;
        if (_ticksGone >= _info.ticksToProduce)
        {
            Fire();
            _ticksGone = 0;
        }
    }

    protected override void Fire()
    {
        move?.Invoke();
        if (_placer.RemoveResources(_info.bulletInfo.resource)) new Laser(_placer, _position, _info.bulletInfo, this);
    }
}