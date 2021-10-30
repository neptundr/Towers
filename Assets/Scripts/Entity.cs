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
        _offset = new Vector2(((info.size.x - 1) / 2f) * ((_placer is null) ? 1 : (_placer.First() ? -1 : 1)), (_info.size.y - 1) / 2f);
        UpdatePicturePosition();
    }

    public Placer GetPlacer()
    {
        return _placer;
    }

    public void Disband()
    {
        GameManager.Destroy(_picture.gameObject);
        AdditionDisbandActions();
    }

    protected virtual void AdditionDisbandActions() {}

    protected Entity() { throw new Exception(); }

    public Vector2Int GetPosition()
    {
        return _position;
    }

    protected void UpdatePicturePosition()
    {
        _picture.transform.position = (Vector2) Tower.This.transform.position + _position + _offset;
    }

    protected void CheckPositionBorders()
    {
        if (_position.x < 0) _position = new Vector2Int(0, _position.y);
        if (_position.y < 0) _position = new Vector2Int(_position.x, 0);
        if (_position.x >= Tower.MapSize.x) _position = new Vector2Int(Tower.MapSize.x - 1, _position.y);
        if (_position.y >= Tower.MapSize.y - 1) _position = new Vector2Int(_position.x, Tower.MapSize.y - 2);
    }
}