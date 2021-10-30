
using System;
using UnityEngine;

public class BlockManager
{
    public static Block MakeBlock(Placer placer, Vector2Int position, BlockInfo blockInfo)
    {
        switch (blockInfo.type)
        {
            case BlockType.Block:
                return new Block(placer, position, blockInfo);
            case BlockType.Platform:
                return new Platform(placer, position, blockInfo);
            case BlockType.ResourceAdder:
                return new ResourceAdder(placer, position, blockInfo);
            case BlockType.WorkerAdder:
                return new WorkerAdder(placer, position, blockInfo);
            case BlockType.Rifle:
                return new Rifle(placer, position, blockInfo as GunInfo);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum BlockType
{
    Block,
    Platform,
    ResourceAdder,
    WorkerAdder,
    Rifle
}
