using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeparatedTimer : MonoBehaviour
{
    [SerializeField] private int _workersTicks = 5;
    [SerializeField] private int _blocksTicks = 2;
    [SerializeField] private int _bulletsTicks = 1;
    [SerializeField] private float _delay = 0.15f;
    
    public event Action SummonWorkers;
    public event Action SummonBlocks;
    public event Action SummonBullets;
    
    public IEnumerator Timer()
    {
        int workersTicksGone = 0;
        int blocksTicksGone = 0;
        int bulletsTicksGone = 0;

        int NextTickGone(int ticksGone, int tickNeeded, EntityType entity)
        {
            int result =  ticksGone + 1 >= tickNeeded ? 0 : ticksGone + 1;

            if (result == 0) Summon(entity);

            return result;
        }
        
        while (true)
        {
            if (!GameManager.BuyPaused())
            {
                workersTicksGone = NextTickGone(workersTicksGone, _workersTicks, EntityType.Worker);
                blocksTicksGone = NextTickGone(blocksTicksGone, _blocksTicks, EntityType.Building);
                bulletsTicksGone = NextTickGone(bulletsTicksGone, _bulletsTicks, EntityType.Bullet);
            }
            yield return new WaitForSeconds(_delay);
        }
    }

    private void Summon(EntityType entityType)
    {
        switch (entityType)
        {
            case EntityType.Worker:
                SummonWorkers?.Invoke();
                // if (_first) SummonWorkersTower1?.Invoke(); 
                // else SummonWorkersTower2?.Invoke();
                break;
            case EntityType.Building:
                SummonBlocks?.Invoke();
                // if (_first) SummonBlocksTower1?.Invoke(); 
                // else SummonBlocksTower2?.Invoke();
                break;
            case EntityType.Bullet:
                SummonBullets?.Invoke();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private enum EntityType
    {
        Worker,
        Building,
        Bullet
    }
}