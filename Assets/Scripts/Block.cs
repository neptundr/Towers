using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Entity
{
    public bool felt;
    public bool justLoaded;
    public float load;
    
    public readonly float mass;

    protected new readonly BlockInfo _info;

    protected bool _projected = true;
    protected float _endurance;
    protected float _health;

    private List<Worker> _workers;
    
    public Block(Placer placer, Vector2Int position, BlockInfo info) : base(placer, position, info)
    {
        _info = info;
        _workers = new List<Worker>();

        mass = info.mass;
        _endurance = info.endurance;
        _health = info.health;
        _picture.color = new Color(_picture.color.r, _picture.color.g, _picture.color.b, 0.25f);

        _placer.Timer.SummonBlocks += TryDoSth;

        // if (_tower.First()) Tower.SummonBlocksTower1 += TryDoSth;
        // else Tower.SummonBlocksTower2 += TryDoSth;
    }

    public void Locate()
    {
        for (int x = 0; x < _info.size.x; x++)
        {
            for (int y = 0; y < _info.size.y; y++)
            {
                Tower.Map[_position.x + (_placer.First() ? -1 : 1) * x, _position.y + y] = this;
                if (this is Surface)
                {
                    if (!(Tower.Surfaces[_position.x + (_placer.First() ? -1 : 1) * x, _position.y + y] is null))
                    {
                        Tower.Surfaces[_position.x + (_placer.First() ? -1 : 1) * x, _position.y + y].Disband();
                    }

                    Tower.Surfaces[_position.x + (_placer.First() ? -1 : 1) * x, _position.y + y] = (Surface) this;
                }
            }
        }
    }

    public void Fall()
    {
        if (!felt)
        {
            felt = true;
            
            Debug.Log(_position);

            for (int x = 0; x < _info.size.x; x++)
            {
                // Debug.Log(new Vector2Int(_position.x + (_tower.First() ? -1 : 1) * x, _position.y - 1));
                
                if (!(Tower.Map[_position.x + (_placer.First() ? -1 : 1) * x, _position.y - 1] is null))
                {
                    Debug.Log("Staying on something");
                    return;
                }
            }

            int highestY = 0;

            for (int x = 0; x < _info.size.x; x++)
            {
                for (int y = _position.y - 1; y >= 0; y--)
                {
                    // Debug.Log(new Vector2Int(x, y));
                    if (!(Tower.Map[_position.x + (_placer.First() ? -1 : 1) * x, y] is null))
                    {
                        if (y > highestY) highestY = y;
                        break;
                    }
                }
            }

            if (_position != new Vector2Int(_position.x, highestY + 1))
            {
                DeleteFromCollections();
                
                _position = new Vector2Int(_position.x, highestY + 1);

                Locate();
                UpdatePicturePosition();
            }
        }
    }

    public virtual void Damage(float damage)
    {
        if (!_projected)
        {
            if (load > _endurance) _health -= damage * (load / _endurance);
            else _health -= damage;

            Debug.Log(_info.health + " / " + _health);

            if (_health <= 0) Disband();
        }
    }

    /// Use 'base' before 'this'
    protected override void AdditionalDisbandActions()
    {
        DeleteFromCollections();
        
        _placer.Timer.SummonBlocks -= TryDoSth;
        
        Tower.SomethingChanged();
        
        // if (_tower.First()) Tower.SummonBlocksTower1 -= TryDoSth;
        // else Tower.SummonBlocksTower2 -= TryDoSth;
    }

    protected virtual void DeleteFromCollections()
    {
        for (int x = 0; x < _info.size.x; x++)
        {
            for (int y = 0; y < _info.size.y; y++)
            {
                Tower.Map[_position.x + (_placer.First() ? -1 : 1) * x, _position.y + y] = null;
            }
        }
    }
    
    private void TryDoSth()
    {
        if (!_projected && _info.isDoingSomething) DoSth();
    }

    protected virtual void DoSth() { Debug.Log("DOING"); }

    // public void Initialize(Vector2Int position, Tower tower)
    // {
    //     _position = position;
    //     _tower = tower;
    // }

    public void ConfirmWork(Worker worker)
    {
        _workers.Add(worker);
        if (_workers.Count >= _info.workersNeeded) Confirm();
    }
    
    protected void Confirm()
    {
        foreach (Worker worker in _workers)
        {
            worker.DisOccupy();
        }
        _workers.Clear();

        _projected = false;
        _picture.color = new Color(_picture.color.r, _picture.color.g, _picture.color.b, 1);

        if (_position.y + _info.size.y >= Tower.MapSize.y - 1 &&
            (_placer.First() ? _position.x >= Tower.MapSize.x / 2 : _position.x < Tower.MapSize.x / 2))
            GameManager.Win(_placer.First());
    }

    protected virtual void AdditionalConfirmActions() {}

    public Vector2Int GetSize()
    {
        return _info.size;
    }
}
