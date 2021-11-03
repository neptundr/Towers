using UnityEngine;

[CreateAssetMenu(fileName = "BulletInfo", menuName = "BulletsInfo", order = 0)]
public class BulletInfo : EntityInfo
{
    [Header("")]
    public float resource;
    public float damage;
}