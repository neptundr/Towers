using System;
using UnityEngine;

public sealed class Ground : Surface
{
    public Ground(Placer placer, Vector2Int position, BlockInfo info) : base(placer, position, info)
    {
        Confirm();
    }

    public override void Damage(float damage) {}

    protected override void AdditionalDisbandActions()
    {
        base.AdditionalDisbandActions();
        throw new Exception();
    }
}