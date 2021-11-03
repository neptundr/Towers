using UnityEngine;

[CreateAssetMenu(fileName = "EntityInfo", menuName = "EntitiesInfo", order = 0)]
public class EntityInfo : ScriptableObject
{
    public Vector2Int size;
    public Sprite picture;
    public int sortingOrder;
    public float spawningSpeed = 5;
    public bool instantlySpawning;
}