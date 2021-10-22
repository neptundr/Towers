using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Entity
{
    public float load;
    
    public readonly float mass;

    protected new readonly BlockInfo _info;

    protected bool _projected = true;
    protected float _endurance;
    protected float _health;

    public Block(Tower tower, Vector2Int position, BlockInfo info) : base(tower, position, info)
    {
        _info = info;

        mass = info.mass;
        _endurance = info.endurance;
        _health = info.health;
        _picture.color = new Color(_picture.color.r, _picture.color.g, _picture.color.b, _info.color.a / 3);

        _tower.SummonBlocks += TryDoSth;

        // if (_tower.First()) Tower.SummonBlocksTower1 += TryDoSth;
        // else Tower.SummonBlocksTower2 += TryDoSth;
    }

    public void Fall()
    {
        for (int x = 0; x < _tower.GetSize().y; x++)
        {
            if (!(_tower.tower[x, _position.y] is null))
            {
                return;
            }
        }

        int highestX = 0;
        int highestY = 0;
        
        for (int x = _position.x; x < _position.x + _info.size.x; x++)
        {
            for (int y = 0; y < _position.y; y++)
            {
                if (!(_tower.tower[(_tower.First() ? -1 : 1) * x, y] is null))
                {
                    if (y > highestY)
                    {
                        highestX = x;
                        highestY = y;
                    }
                }
            }
        }

        _position = new Vector2Int(highestX, highestY);
    }
    
    public void Damage(float damage)
    {
        if (load > _endurance) _health -= damage * (load / _endurance);
        else _health -= damage;
    }
    
    public bool GetProjected()
    {
        return _projected;
    }
    
    /// Use 'base' before 'this'
    protected override void AdditionDisbandActions()
    {
        DeleteFromCollections();
        
        _tower.SummonBlocks -= TryDoSth;
        
        _tower.SomethingChanged();
        
        // if (_tower.First()) Tower.SummonBlocksTower1 -= TryDoSth;
        // else Tower.SummonBlocksTower2 -= TryDoSth;
    }

    protected virtual void DeleteFromCollections()
    {
        for (int x = 0; x < _info.size.x; x++)
        {
            for (int y = 0; y < _info.size.y; y++)
            {
                _tower.tower[_position.x + (_tower.First() ? -1 : 1) * x, _position.y + y] = null;
            }
        }
    }
    
    private void TryDoSth()
    {
        if (!_projected && _info.isDoingSomething) DoSth();
    }

    protected virtual void DoSth(){ Debug.Log("DOING"); }

    // public void Initialize(Vector2Int position, Tower tower)
    // {
    //     _position = position;
    //     _tower = tower;
    // }
    
    public void Confirm()
    {
        _projected = false;
        _picture.color = new Color(_picture.color.r, _picture.color.g, _picture.color.b, _info.color.a);
    }
    
    public Vector2Int GetSize()
    {
        return _info.size;
    }
}
