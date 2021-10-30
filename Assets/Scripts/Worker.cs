using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Worker : Entity
{
    private bool _occupied;
    private bool _confirmed;
    private Block _block;
    private Vector2Int _destination;
    private JobType _jobType;
    private Surface _onWhatIsStanding;

    public Worker(Placer placer, Vector2Int position, EntityInfo info) : base(placer, position, info)
    {
        Fall();
        
        _placer.Timer.SummonWorkers += TryDoJob;
        
        // if (_tower.First()) Tower.SummonWorkersTower1 += TryDoJob;
        // else Tower.SummonWorkersTower2 += TryDoJob;
    }

    public void Fall()
    {
        if (Tower.Surfaces[_position.x, _position.y - 1] is null)
        {
            for (int i = 0; i < _position.y; i++)
            {
                if (!(Tower.Surfaces[_position.x, _position.y - i - 1] is null))
                {
                    Move(new Vector2Int(0, -i));
                }
            }
        }
    }

    public bool GetOccupied()
    {
        return _occupied;
    }

    public void DisOccupy()
    {
        _occupied = false;
        _confirmed = false;
    }

    public void SetOccupation(Block block, Vector2Int destination, JobType whatToDo)
    {
        _occupied = true;
        _confirmed = false;
        
        _block = block;
        _destination = destination;
        _jobType = whatToDo;
    }

    private void TryDoJob()
    {
        if (_occupied) DoJob();
    }

    private void DoJob()
    {
        if (_jobType == JobType.Build && !_confirmed)
        {
            if (_position == _destination)
            {
                Tower.Map[_position.x, _position.y].ConfirmWork(this);
                _confirmed = true;
            }
            else
            {
                if (Mathf.Abs(_destination.x - _position.x) > Mathf.Abs(_destination.y - _position.y))
                    Move(new Vector2Int((_destination.x > _position.x ? 1 : -1), 0));
                else Move(new Vector2Int(0, (_destination.y > _position.y ? 1 : -1)));
            }
        }
    }

    protected override void AdditionDisbandActions()
    {
        Tower.RemoveWorker(this, _placer.First());
        
        _placer.Timer.SummonWorkers -= TryDoJob;
        
        // if (_tower.First()) Tower.SummonWorkersTower1 -= TryDoJob;
        // else Tower.SummonWorkersTower2 -= TryDoJob; 
    }

    private void Move(Vector2Int direction)
    {
        _position += direction;
        _onWhatIsStanding?.Exit(this);

        _onWhatIsStanding = Tower.UpdateAssistPlatform(_position + new Vector2Int(0, -1), _placer.First());
        _onWhatIsStanding.StandOn(this);
        CheckPositionBorders();
        UpdatePicturePosition();
    }

    public enum JobType
    {
        Build,
        Work
    }
}
