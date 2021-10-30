using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockInfo", menuName = "BlocksInfo", order = 1)]
public class BlockInfo : EntityInfo
{
    public float resource1;
    public float resource2;
    public float resource3;
    public float mass;
    public float endurance;
    public float health;
    public BlockType type;

    [Header("Workers needed is equal offsets length. Offset is less than size")]
    public uint workersNeeded;
    public Vector2Int[] offsets;

    [Header("")]
    public bool isDoingSomething;
    public int ticksToProduce;
}