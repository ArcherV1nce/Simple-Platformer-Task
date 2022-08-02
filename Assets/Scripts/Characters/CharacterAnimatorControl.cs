using UnityEngine;

[RequireComponent(typeof(Movement), typeof(Animator))]
public class CharacterAnimatorControl : MonoBehaviour
{
    [SerializeField, Range (0, 1f)] private float _minimumFlipVelocity = 0.1f;

    private const string RunningParameter = "IsRunning";
    private const string MovingLeftParameter = "MovingLeft";
    private const string GroundedParameter = "IsGrounded";

    private Animator _animator;
    private Movement _movement;
    private bool _isMoving;
    private bool _directionLeft;

    private void Awake()
    {
        SetUp();
    }

    private void FixedUpdate()
    {
        CheckForMovementHorizontal();
        CheckForMovementVertical();
    }

    private void CheckForMovementHorizontal()
    {
        if (Mathf.Abs(_movement.Velocity.x) > 0)
        {
            _isMoving = true;

            if (_movement.Velocity.x < -_minimumFlipVelocity)
            {
                _directionLeft = true;
            }
            
            else
            {
                _directionLeft = false;
            }
        }

        else
        {
            _isMoving = false;
        }

        _animator.SetBool(RunningParameter, _isMoving);
        _animator.SetBool(MovingLeftParameter, _directionLeft);
    }
    private void CheckForMovementVertical()
    {
        _animator.SetBool(GroundedParameter, _movement.IsGrounded);
    }

    private void SetUp()
    {
        _animator = GetComponent<Animator>();
        _movement = GetComponent<Movement>();
    }
}