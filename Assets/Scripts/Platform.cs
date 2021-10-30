using UnityEngine;

public class Platform : Surface
{
    public Platform(Placer placer, Vector2Int position, BlockInfo info) : base(placer, position, info) {}

    protected override void DeleteFromCollections()
    {
        base.DeleteFromCollections();
        DeleteFromTower();
    }
}