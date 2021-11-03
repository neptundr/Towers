using System;
using UnityEngine;

public class PicturePosition : MonoBehaviour
{
    private float _speed = 5;
    private Vector2 _destination;

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    public void SetDestination(Vector2 destination)
    {
        _destination = destination;
    }
    
    public void SetPosition(Vector2 position)
    {
        SetDestination(position);
        transform.position = position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _destination, _speed / 10f);
    }
}
