using System;
using UnityEngine;

public class Laser : Bullet
{
    private LaserGun _laserGun;
    
    public Laser(Placer placer, Vector2Int position, BulletInfo info, LaserGun laserGun) : base(placer, position, info)
    {
        _laserGun = laserGun;
        _placer.Timer.SummonBullets -= Move;
        _laserGun.move += Move;
    }

    protected override void AdditionalDisbandActions()
    {
        base.AdditionalDisbandActions();
        
        _laserGun.move -= Move;
    }

    protected override void Move()
    {
        _position = new Vector2Int(_position.x, _position.y + 1);
        base.Move();
    }
}