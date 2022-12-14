using System;
using UnityEngine;
using System.Collections.Generic;

public class EnemyMovement : Movement
{
    [SerializeField] private List<Transform> _waypoints;
    [SerializeField, Range(0,1f)] private float _waypointPadding = 0.2f;

    private int _currentWaypoint = 0;

    public Vector2 CurrentPosition => transform.position;

    private void OnValidate()
    {
        ValidateWaypoints();
    }

    protected override void Update()
    {
        ChooseWaypoint();
        base.Update();
    }
    
    private void ChooseWaypoint()
    {
        Vector2 desiredPosition = _waypoints[_currentWaypoint].position;

        if (Mathf.Abs(Mathf.Abs(CurrentPosition.x) - Mathf.Abs(desiredPosition.x)) <= _waypointPadding)
        {
            if (_currentWaypoint + 1 < _waypoints.Count)
            {
                _currentWaypoint++;
            }

            else
            {
                _currentWaypoint = 0;
            }
        }
    }

    private int ChooseHorizontalDirection()
    {
        Vector2 currentPosition = transform.position;
        Vector2 desiredPosition = _waypoints[_currentWaypoint].position;

        float determinant = (desiredPosition.x - currentPosition.x) * (currentPosition.y + 1 - currentPosition.y) - 
            (desiredPosition.y - currentPosition.y) * (currentPosition.x - currentPosition.x);

        return GetSign(determinant);
    }

    private int GetSign (float number)
    {
        if (number > 0)
            return 1;

        else if (number < 0)
            return -1;

        else return 0;
    }

    private void ValidateWaypoints ()
    {
        if (_waypoints.Count < 1)
        {
            _waypoints = new List<Transform>
            {
                gameObject.transform
            };
        }
    }

    protected override void ChooseDirection()
    {
        bool isJumping = false;
        Vector2 direction = new Vector2(ChooseHorizontalDirection(), 0);

        SetDirection(direction, isJumping);
    }
}