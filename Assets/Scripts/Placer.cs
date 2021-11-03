using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class Placer : Entity
{
    private Image _whatToPlaceImage;
    private Text _amountText;
    private Text _resourceText;

    public readonly SeparatedTimer Timer;
    public readonly Material Material;
    
    private readonly bool _first;

    private float _minResourcesCount = -50f;
    private uint _blockToProjectIndex;
    private float _resources;
    private List<BlockInfoAndAmount> _availableBlocks;
    private Camera _camera;
    private CameraMovement _cameraMovement;

    public Placer(Placer placer, Vector2Int position, EntityInfo info, Camera camera, Image image, Text text, bool first, SeparatedTimer timer, Material material, Text resourceText) : base(placer, position, info)
    {
        _resources = GameManager.START_RESOURCES;
        Material = material;
        Timer = timer;
        _first = first;
        _camera = camera;
        _cameraMovement = camera.GetComponent<CameraMovement>();
        _whatToPlaceImage = image;
        _amountText = text;
        _resourceText = resourceText;
        _picture.material = Material;
        _availableBlocks = new List<BlockInfoAndAmount>();
        uint[] startBlocksCount = GameManager.GetStartPackCount();
        BlockInfo[] startBlocksInfo = GameManager.GetStartPackInfo();
        for (int i = 0; i < startBlocksCount.Length; i++)
        {
            _availableBlocks.Add(new BlockInfoAndAmount(startBlocksCount[i], startBlocksInfo[i]));
        }
        ChangeBlock();
        _cameraMovement.SetTarget(_picture.transform);
    }

    public bool First()
    {
        return _first;
    }

    public float GetResources()
    {
        return _resources;
    }

    public void AddResources(float amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException();

        _resources += amount;
        UpdateResourceText();
    }
    public bool RemoveResources(float amount)
    {
        if (_resources - amount >= _minResourcesCount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException();

            _resources -= amount;
            UpdateResourceText();
            
            return true;
        }

        return false;
    }

    private void UpdateResourceText()
    {
        _resourceText.text = _resources.ToString();
    }
    
    public void PlaceOrder()
    {
        if (Tower.Map[_position.x, _position.y] is IOrderPlaceable)
        {
            if (Tower.Map[_position.x, _position.y].GetPlacer() == this)
            {
                ((IOrderPlaceable) Tower.Map[_position.x, _position.y]).PlaceOrder();
            }
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
            if (Tower.CanPlace(_position, _availableBlocks[(int) _blockToProjectIndex].info.size, _availableBlocks[(int) _blockToProjectIndex].info.workersNeeded, _first))
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

    protected override void AdditionalDisbandActions()
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