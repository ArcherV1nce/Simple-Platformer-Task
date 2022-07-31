using UnityEngine;

[RequireComponent(typeof(Movement), typeof(Animator))]
public class AnimatorControl : MonoBehaviour
{
    [SerializeField, Range (0, 1f)] private float _minimumFlipVelocity = 0.1f;

    private Animator _animator;
    private Movement _characterMovement;
    private bool _isMoving;
    private bool _directionLeft;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _characterMovement = GetComponent<Movement>();
    }

    private void FixedUpdate()
    {
        CheckForMovementHorizontal();
        CheckForMovementVertical();
    }

    private void CheckForMovementHorizontal()
    {
        if (Mathf.Abs(_characterMovement.Velocity.x) > 0)
        {
            _isMoving = true;

            if (_characterMovement.Velocity.x < -_minimumFlipVelocity)
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

        _animator.SetBool("IsRunning", _isMoving);
        _animator.SetBool("MovingLeft", _directionLeft);
    }
    private void CheckForMovementVertical()
    {
        _animator.SetBool("IsGrounded", _characterMovement.IsGrounded);
    }
}