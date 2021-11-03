
using UnityEngine;

public class Grapper : Gun, IOrderPlaceable
{
    private static float _damage = 1;

    private bool _grappedSomethingBefore;
    private int _ticksGone;
    private Block _prey;

    public Grapper(Placer placer, Vector2Int position, BlockInfo info) : base(placer, position, info) {}

    protected override void AdditionalConfirmActions()
    {
        ReLocate();
    }

    private void ReLocate()
    {
        DeleteFromCollections();

        _prey = null;
        for (int x = 0; x < _position.x; x++)
        {
            if (!(Tower.Map[_position.x + (_placer.First() ? -1 : 1) * x, _position.y] is null))
            {
                _prey = Tower.Map[_position.x + (_placer.First() ? -1 : 1) * x, _position.y];
                break;
            }
        }
        
        if (!(_prey is null))
        {
            _position = _prey.GetPosition() + new Vector2Int((_placer.First() ? 1 : -1), 0);
            UpdatePicturePosition();
        }
    }

    protected override void DoSth()
    {
        if (_prey is null)
        {
            for (int x = 1; x < (_placer.First() ? _position.x : Tower.MapSize.x - _position.x); x++)
            {
                if (!(Tower.Map[_position.x + (_placer.First() ? -1 : 1) * x, _position.y] is null))
                {
                    DeleteFromCollections();
                    _prey = Tower.Map[_position.x + (_placer.First() ? -1 : 1) * x, _position.y];
                    _position = _prey.GetPosition() + new Vector2Int((_placer.First() ? 1 : -1), 0);
                    Locate();
                    Tower.SomethingChanged();
                    UpdatePicturePosition();
                    break;
                }
            }

            if (_prey is null)
            {
                Vector2Int _prePostition = _position;
                Fall();
                if (_position == _prePostition) Disband();
            }
        }
        else
        {
            _ticksGone += 1;
            if (_ticksGone >= _info.ticksToProduce)
            {
                _prey.Damage(_damage);
                _ticksGone = 0;
                if (Tower.Map[_prey.GetPosition().x, _prey.GetPosition().y] is null || Tower.Map[_prey.GetPosition().x, _prey.GetPosition().y] != _prey)
                {
                    _prey = null;
                }
            }
        }
    }

    public void PlaceOrder()
    {
        Disband();
    }
}