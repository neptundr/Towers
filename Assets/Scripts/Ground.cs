using System;
using UnityEngine;

public sealed class Ground : Surface
{
    public Ground(Placer placer, Vector2Int position, BlockInfo info) : base(placer, position, info)
    {
        Confirm();
    }

    protected override void AdditionDisbandActions()
    {
        base.AdditionDisbandActions();
        throw new Exception();
    }
}