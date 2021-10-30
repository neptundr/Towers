using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Placer : Entity
{
    [SerializeField] private Image _whatToPlaceImage;
    [SerializeField] private Text _amountText;

    public readonly SeparatedTimer Timer;
    public readonly Material Material;
    
    private readonly bool _first;
    
    private uint _blockToProjectIndex;
    private float _startImageScale;
    private List<BlockInfoAndAmount> _availableBlocks;
    private Camera _camera;

    public Placer(Placer placer, Vector2Int position, EntityInfo info, Camera camera, Image image, Text text, bool first, SeparatedTimer timer, Material material) : base(placer, position, info)
    {
        Material = material;
        Timer = timer;
        _first = first;
        _camera = camera;
        _whatToPlaceImage = image;
        _amountText = text;
        _picture.material = Material;
        _availableBlocks = new List<BlockInfoAndAmount>();
        uint[] startBlocksCount = GameManager.GetStartPackCount();
        BlockInfo[] startBlocksInfo = GameManager.GetStartPackInfo();
        for (int i = 0; i < startBlocksCount.Length; i++)
        {
            _availableBlocks.Add(new BlockInfoAndAmount(startBlocksCount[i], startBlocksInfo[i]));
        }
        _startImageScale = _whatToPlaceImage.rectTransform.sizeDelta.x;
        ChangeBlock();
        UpdateCameraPosition();
    }

    public bool First()
    {
        return _first;
    }

    public void PlaceOrder()
    {
        if (Tower.Map[_position.x, _position.y] is IOrderPlaceable)
        {
            ((IOrderPlaceable) Tower.Map[_position.x, _position.y]).PlaceOrder();
        }
    }

    public void AddBlock(BlockInfo blockInfo)
    {
        for (int i = 0; i < _availableBlocks.Count; i++)
        {
            if (_availableBlocks[i].info == blockInfo)
            {
                _availableBlocks[i].AddAmount(1);
                return;
            }
        }

        _availableBlocks.Add(new BlockInfoAndAmount(1, blockInfo));
    }
    
    public void DeleteAssistPlatform()
    {
        if (!(Tower.Surfaces[_position.x, _position.y] is null))
        {
            if (Tower.Surfaces[_position.x, _position.y] is AssistPlatform)
            {
                Tower.Surfaces[_position.x, _position.y].Disband();
            }
        }
    }

    public void Move(Vector2Int direction)
    {
        _position = new Vector2Int(_position.x + direction.x, _position.y + direction.y);
        
        CheckPositionBorders();
        UpdatePicturePosition();
        UpdateCameraPosition();
    }

    public void ChangeBlock()
    {
        if (_availableBlocks.Count != 0)
        {
            _blockToProjectIndex = _blockToProjectIndex + 1 >= _availableBlocks.Count ? 0 : _blockToProjectIndex + 1;

            UpdateUI();
        }
        else
        {
            _whatToPlaceImage.color = new Color(0, 0, 0, 0);
            _whatToPlaceImage = null;
            _amountText.text = "";
        }
    }

    private void UpdateUI()
    {
        _whatToPlaceImage.sprite = _availableBlocks[(int) _blockToProjectIndex].info.picture;
        _whatToPlaceImage.color = new Color(1, 1, 1, 1);
        _whatToPlaceImage.preserveAspect = true;

        _amountText.text = _availableBlocks[(int) _blockToProjectIndex].GetAmount().ToString();
    }

    public void TryPlace()
    {
        if (_availableBlocks.Count != 0)
        {
            if (Tower.CanPlace(_position, _availableBlocks[(int) _blockToProjectIndex].info.size, (uint) _availableBlocks[(int) _blockToProjectIndex].info.workersNeeded, _first))
            {
                Block block = BlockManager.MakeBlock(this, _position, _availableBlocks[(int) _blockToProjectIndex].info);
                Tower.ProjectBlock(_position, block, _availableBlocks[(int) _blockToProjectIndex].info, _first);

                if (_availableBlocks[(int) _blockToProjectIndex].SubAmount(1) <= 0)
                {
                    _availableBlocks.RemoveAt((int) _blockToProjectIndex);
                    ChangeBlock();
                }
                else
                {
                    UpdateUI();
                }
            }
        }
    }

    protected override void AdditionDisbandActions()
    {
        throw new Exception();
    }

    public void MaximizeCamera()
    {
        _camera.orthographicSize = _camera.orthographicSize >= GameManager.MAX_CAMERA_SIZE
            ? _camera.orthographicSize //GameManager.MIN_CAMERA_SIZE
            : _camera.orthographicSize + GameManager.CAMERA_CHANGE_SPEED;
    }

    public void MinimizeCamera()
    {
        _camera.orthographicSize = _camera.orthographicSize <= GameManager.MIN_CAMERA_SIZE
            ? _camera.orthographicSize //GameManager.MAX_CAMERA_SIZE
            : _camera.orthographicSize - GameManager.CAMERA_CHANGE_SPEED;
    }

    private void UpdateCameraPosition()
    {
        _camera.transform.position = Tower.This.transform.position + new Vector3(_position.x, _position.y, GameManager.CAMERA_Z_POSITION);
    }
}

[Serializable]
public class BlockInfoAndAmount
{
    public readonly BlockInfo info;
    private uint _amount;

    public BlockInfoAndAmount(uint amount, BlockInfo info)
    {
        this._amount = amount;
        this.info = info;
    }

    public uint GetAmount()
    {
        return _amount;
    }

    public uint AddAmount(uint count)
    {
        if (count <= 0) throw new ArgumentOutOfRangeException();

        _amount += count;

        return _amount;
    }

    public uint SubAmount(uint count)
    {
        if (count <= 0) throw new ArgumentOutOfRangeException();

        _amount -= count;

        return _amount;
    }
}