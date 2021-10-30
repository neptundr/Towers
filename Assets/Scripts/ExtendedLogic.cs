using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class ExtendedLogic : MonoBehaviour
{
    public static Entity FindNearestIn(Entity[] array, Vector2Int to, Vector2Int towerSize)
    {
        if (array.Length == 0) throw new ArgumentOutOfRangeException();
        
        Entity min = null;
        float minDistance = towerSize.x + towerSize.y + 1;

        foreach (var entity in array)
        {
            int probablyMinDistance = Mathf.Abs(entity.GetPosition().x - to.x) +
                                      Mathf.Abs(entity.GetPosition().y - to.y);

            if (probablyMinDistance < minDistance)
            {
                minDistance = probablyMinDistance;
                min = entity;
            }
        }

        return min;
    }

    public static Entity FindNearestIn(List<Entity> list, Vector2Int to, Vector2Int towerSize)
    {
        return FindNearestIn(list.ToArray(), to, towerSize);
    }

    public static Entity FindNearestIn(Entity[,] array, Vector2Int to, Vector2Int towerSize)
    {
        List<Entity> entities = new List<Entity>();

        for (int x = 0; x < towerSize.x; x++)
        {
            for (int y = 0; y < towerSize.y; y++)
            {
                if (!(array[x, y] is null)) entities.Add(array[x, y]);
            }
        }

        return FindNearestIn(entities.ToArray(), to, towerSize);
    }
}