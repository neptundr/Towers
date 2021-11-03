using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity
{
    protected readonly EntityInfo _info;
    protected Sprite _defaultSprite;
    protected Placer _placer;
    protected Vector2 _offset;
    protected Vector2Int _position;
    protected SpriteRenderer _picture;
    protected PicturePosition _picturePosition;

    public Entity(Placer placer, Vector2Int position, EntityInfo info)
    {
        _info = info;
        _defaultSprite = _info.picture;
        _placer = placer;
        _position = position;
        _picture = (SpriteRenderer) GameManager.Instantiate(GameManager.GetEmpty()).AddComponent(typeof(SpriteRenderer));
        _picture.sprite = _defaultSprite;
        _picture.material = _placer?.Material ? _placer?.Material : null;
        // _picture.transform.localScale = new Vector3(info.size.x, info.size.y);
        _picture.sortingOrder = _info.sortingOrder;
        _picture.flipX = !_placer?.First() ?? false;
        _picturePosition = _picture.gameObject.AddComponent<PicturePosition>();
        _offset = new Vector2(((info.size.x - 1) / 2f) * ((_placer is null) ? 1 : (_placer.First() ? -1 : 1)), (_info.size.y - 1) / 2f);
        _picturePosition.SetSpeed(_info.spawningSpeed);
        if (_info.instantlySpawning) _picturePosition.SetPosition((Vector2) Tower.This.transform.position + _position + _offset);
        else
        {
            if (!(_placer is null))
            {
                if (!_placer.First())
                    _picturePosition.SetPosition(new Vector2(Tower.This.transform.position.x + Tower.MapSize.x - 1, 0));
            }

            UpdatePicturePosition();
        }

        // _picture.transform.position = (Vector2) Tower.This.transform.position + _position + _offset;
    }

    public Placer GetPlacer()
    {
        return _placer;
    }

    public void Disband()
    {
        if (!(_picture.gameObject is null)) GameManager.Destroy(_picture.gameObject);
        AdditionalDisbandActions();
    }

    protected virtual void AdditionalDisbandActions() {}

    protected Entity() { throw new Exception(); }

    public Vector2Int GetPosition()
    {
        return _position;
    }

    protected void UpdatePicturePosition()
    {
        _picturePosition.SetDestination((Vector2) Tower.This.transform.position + _position + _offset);
    }

    protected void CheckPositionBorders()
    {
        if (_position.x < 0) _position = new Vector2Int(0, _position.y);
        if (_position.y < 0) _position = new Vector2Int(_position.x, 0);
        if (_position.x >= Tower.MapSize.x) _position = new Vector2Int(Tower.MapSize.x - 1, _position.y);
        if (_position.y >= Tower.MapSize.y) _position = new Vector2Int(_position.x, Tower.MapSize.y - 1);
    }
}