using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    public const int BLOCKS_TO_BUY_COUNT = 5;
    public const uint START_WORKERS_AMOUNT = 5;
    public const float CAMERA_Z_POSITION = -100;
    public const float MAX_CAMERA_SIZE = 25;
    public const float MIN_CAMERA_SIZE = 5;
    public const float CAMERA_CHANGE_SPEED = 0.35f;
    public const float WORK_DAY_TIME = 30;

    private static GameManager This;
    private static bool _buyPaused;

    [SerializeField] private BlockInfo[] _startBlocksInfo; 
    [SerializeField] private uint[] _startBlocksCount;
    [Header("")]
    [SerializeField] private BuyMenu _buyMenu;
    [SerializeField] private SeparatedTimer _firstTimer;
    [SerializeField] private SeparatedTimer _secondTimer;
    [SerializeField] private Tower _tower;
    [SerializeField] private GameObject _empty;
    [SerializeField] private EntityInfo _placer;
    [SerializeField] private EntityInfo _worker;
    [SerializeField] private BlockInfo _assistPlatform;
    [SerializeField] private BlockInfo _ground;

    private bool _firstConfirmed;
    private bool _secondConfirmed;

    public static Action Trigger;

    // public static Tower AnotherTower(Tower tower)
    // {
    //     return This._tower1 == tower ? This._tower2 : This._tower1;
    // }
    
    public static BlockInfo[] GetStartPackInfo()
    {
        return This._startBlocksInfo;
    }

    public static uint[] GetStartPackCount()
    {
        return This._startBlocksCount;
    }

    public static GameObject GetEmpty()
    {
        return This._empty;
    }

    public static EntityInfo Placer()
    {
        return This._placer;
    }

    public static EntityInfo Worker()
    {
        return This._worker;
    }

    public static BlockInfo Ground()
    {
        return This._ground;
    }

    public static BlockInfo AssistPlatform()
    {
        return This._assistPlatform;
    }

    // public static GameObject InstantiateSomething(GameObject whatToInstantiate)
    // {
    //     return Instantiate(whatToInstantiate);
    // }
    //
    // public static void DestroySomething(GameObject whatToDestroy)
    // {
    //     Destroy(whatToDestroy);
    // }
    
    private void Start()
    {
        This = this;

        if (_startBlocksCount.Length != _startBlocksInfo.Length) throw new ArgumentOutOfRangeException();

        _buyMenu.gameObject.SetActive(false);
        
        _tower.LateStart(_firstTimer, _secondTimer);

        StartCoroutine(_firstTimer.Timer());
        StartCoroutine(_secondTimer.Timer());
        
        Invoke(nameof(BuyPause), WORK_DAY_TIME);
    }

    private void BuyPause()
    {
        Debug.Log("Buy pause");
        _buyPaused = true;
        Trigger?.Invoke();
        BuyMenu.Init();
    }

    public static void ConfirmBuy(bool first)
    {
        if (first) This._firstConfirmed = true;
        else This._secondConfirmed = true;
        if (This._firstConfirmed && This._secondConfirmed)
        {
            This._firstConfirmed = false;
            This._secondConfirmed = false;
            This.Play();
        }
    }

    private void Play()
    {
        _buyPaused = false;
        BuyMenu.DisInit();
        Invoke(nameof(BuyPause), WORK_DAY_TIME);
    }

    public static bool BuyPaused()
    {
        return _buyPaused;
    }
}