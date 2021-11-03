using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BuyMenu : MonoBehaviour
{
    [SerializeField] private BlockInfo[] _allBlockInfos;
    [SerializeField] private Image[] _images;
    [SerializeField] private Text[] _texts;
    [SerializeField] private Image _showerFirst;
    [SerializeField] private Image _showerNonFirst;

    private static BuyMenu _this;

    private int _showerFirstChoose;
    private int _showerNonFirstChoose;
    private BlockInfo[] _blocksToBuy;

    private void Start()
    {
        _this = this;
    }

    public static void Init()
    {
        _this.gameObject.SetActive(true);
        _this._blocksToBuy = new BlockInfo[GameManager.BLOCKS_TO_BUY_COUNT];
        for (int i = 0; i < GameManager.BLOCKS_TO_BUY_COUNT; i++)
        {
            FillAllInIndex(i);
        }
        _this._showerFirst.transform.position = _this._images[_this._showerFirstChoose].transform.position;
        _this._showerNonFirst.transform.position = _this._images[_this._showerNonFirstChoose].transform.position;
    }

    private static void FillAllInIndex(int i)
    {
        _this._blocksToBuy[i] = _this._allBlockInfos[Random.Range(0, _this._allBlockInfos.Length)];
        _this._images[i].sprite = _this._blocksToBuy[i].picture;
        _this._images[i].preserveAspect = true;
        _this._texts[i].text = _this._blocksToBuy[i].resource.ToString();
    }

    public static void DisInit()
    {
        _this.gameObject.SetActive(false);
    }

    public static void Buy(Placer placer)
    {
        if (placer.RemoveResources(_this
            ._blocksToBuy[placer.First() ? _this._showerFirstChoose : _this._showerNonFirstChoose].resource))
        {
            placer.AddBlock(
                _this._blocksToBuy[placer.First() ? _this._showerFirstChoose : _this._showerNonFirstChoose]);
            FillAllInIndex(placer.First() ? _this._showerFirstChoose : _this._showerNonFirstChoose);
        }
    }
    
    public static void SetShower(Direction direction, bool first)
    {
        if (first)
        {
            _this._showerFirstChoose += direction == Direction.Right ? 1 : -1;
            if (_this._showerFirstChoose >= GameManager.BLOCKS_TO_BUY_COUNT) _this._showerFirstChoose = 0;
            if (_this._showerFirstChoose < 0) _this._showerFirstChoose = GameManager.BLOCKS_TO_BUY_COUNT - 1;
            _this._showerFirst.transform.position = _this._images[_this._showerFirstChoose].transform.position;
        }
        else
        {
            _this._showerNonFirstChoose += direction == Direction.Right ? 1 : -1;
            if (_this._showerNonFirstChoose >= GameManager.BLOCKS_TO_BUY_COUNT) _this._showerNonFirstChoose = 0;
            if (_this._showerNonFirstChoose < 0) _this._showerNonFirstChoose = GameManager.BLOCKS_TO_BUY_COUNT - 1;
            _this._showerNonFirst.transform.position = _this._images[_this._showerNonFirstChoose].transform.position;
        }
    }
}

public enum Direction
{
    Right,
    Left
}