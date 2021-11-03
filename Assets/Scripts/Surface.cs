using System.Collections.Generic;
using UnityEngine;

public abstract class Surface : Block
{
    protected List<Worker> _whoIsStanding = new List<Worker>();
    
    public Surface(Placer placer, Vector2Int position, BlockInfo info) : base(placer, position, info) {}

    public void StandOn(Worker worker)
    {
        _whoIsStanding.Add(worker);
    }
    
    public void Exit(Worker worker)
    {
        _whoIsStanding.Remove(worker);
        OnExit();
    }

    protected virtual void OnExit(){}

    protected override void AdditionalDisbandActions()
    {
        base.AdditionalDisbandActions();
        for (int i = 0; i < _whoIsStanding.Count; i++)
        {
            _whoIsStanding[i].Fall();
        }
    }
    
    protected override void DeleteFromCollections()
    {
        Tower.Surfaces[_position.x, _position.y] = null;
    }
    protected void DeleteFromTower()
    {
        base.DeleteFromCollections();
    }
}