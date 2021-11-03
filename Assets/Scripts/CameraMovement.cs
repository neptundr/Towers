using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _smooth = 5;
    
    private Transform _target;

    public void SetTarget(Transform transform)
    {
        _target = transform;
    }
    
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position,
            _target.position + new Vector3(0, 0, GameManager.CAMERA_Z_POSITION), Time.deltaTime * _smooth);
    }
}