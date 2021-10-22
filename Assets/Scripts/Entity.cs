using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity
{
    protected readonly EntityInfo _info;
    protected Sprite _defaultSprite;
    protected Tower _tower;
    protected Vector2 _offset;
    protected Vector2Int _position;
    protected SpriteRenderer _picture;

    public Entity(Tower tower, Vector2Int position, EntityInfo info)
    {
        _info = info;
        _defaultSprite = _info.picture;
        _tower = tower;
        _position = position;
        _picture = (SpriteRenderer) GameManager.Instantiate(GameManager.GetEmpty()).AddComponent(typeof(SpriteRenderer));
        _picture.sprite = _defaultSprite;
        _picture.color = info.color;
        _picture.transform.localScale = new Vector3(info.size.x, info.size.y);
        _picture.sortingOrder = _info.sortingOrder;
        _offset = new Vector2(((info.size.x - 1) / 2f) * (_tower.First() ? -1 : 1), (_info.size.y - 1) / 2f);
        UpdatePicturePosition();
    }

    public void Disband()
    {
        AdditionDisbandActions();
        GameManager.Destroy(_picture.gameObject);
    }

    protected virtual void AdditionDisbandActions() {}

    protected Entity() { throw new Exception(); }

    public Vector2Int GetPosition()
    {
        return _position;
    }

    protected void UpdatePicturePosition()
    {
        _picture.transform.position = (Vector2) _tower.transform.position + _position + _offset;
    }

    protected void CheckPositionBorders()
    {
        if (_position.x < 0) _position = new Vector2Int(0, _position.y);
        if (_position.y < 0) _position = new Vector2Int(_position.x, 0);
        if (_position.x >= _tower.GetSize().x) _position = new Vector2Int(_tower.GetSize().x - 1, _position.y);
        if (_position.y >= _tower.GetSize().y) _position = new Vector2Int(_position.x, _tower.GetSize().y - 1);
    }
}