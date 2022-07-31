using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Movement : MonoBehaviour
{
    private const int HitBufferSize = 16;
    private const float MinMoveDistance = 0.01f;
    private const float ShellRadius = 0.01f;

    private Rigidbody2D _rigidbody;
    private bool _isGrounded;
    private ContactFilter2D _contactFilter2D;
    private readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[HitBufferSize];
    private readonly List<RaycastHit2D> _hitList = new (HitBufferSize);
    protected Vector2 _targetVelocity;
    private Vector2 _groundNormal;
    private Vector2 _velocity;

    [SerializeField] protected float _horizontalMovementVelocity = 5;
    [SerializeField] protected float _jumpVelocity = 15;
    [SerializeField] protected LayerMask _layerMask;
    [SerializeField] protected float _gravityModifier = 2f;
    [SerializeField] protected float _minGroundNormalY = 0.5f;

    public bool IsGrounded => _isGrounded;
    public Vector2 Velocity => _velocity;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _contactFilter2D.useTriggers = false;
        _contactFilter2D.SetLayerMask(_layerMask);
        _contactFilter2D.useLayerMask = true;
    }

    protected virtual void Update()
    {
        ChooseDirection();
    }
    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected void SetDirection(Vector2 horizontalDirection, bool isJumping)
    {
        _targetVelocity = horizontalDirection;

        Jump(isJumping);
    }

    protected void Jump(bool isJumping)
    {
        if (isJumping)
        {
            if (_isGrounded)
            {
                _velocity.y = _jumpVelocity;
            }
        } 
    }

    protected virtual void Move ()
    {
        _velocity += _gravityModifier * Time.deltaTime * Physics2D.gravity;
        _velocity.x = _targetVelocity.x * _horizontalMovementVelocity;

        _isGrounded = false;

        Vector2 deltaPosition = _velocity * Time.deltaTime;
        Vector2 movementAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
        Vector2 movement = movementAlongGround * deltaPosition.x;

        MoveByDirection(movement, false);

        movement = Vector2.up * deltaPosition.y;

        MoveByDirection(movement, true);
    }

    protected void MoveByDirection(Vector2 movement, bool yMovement)
    {
        float distance = movement.magnitude;

        if (distance > MinMoveDistance)
        {

            int count = _rigidbody.Cast(movement, _contactFilter2D, _hitBuffer, distance + ShellRadius);

            _hitList.Clear();

            for (int i = 0; i < count; i++)
            {
                _hitList.Add(_hitBuffer[i]);
            }

            for (int i = 0; i < _hitList.Count; i++)
            {
                Vector2 currentNormal = _hitList[i].normal;

                if (currentNormal.y > _minGroundNormalY)
                {
                    _isGrounded = true;
                    if (yMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(_velocity, currentNormal);

                if (projection < 0)
                {
                    _velocity = _velocity - projection * currentNormal;
                }

                float modifiedDistance = _hitList[i].distance - ShellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        _rigidbody.position = _rigidbody.position + movement.normalized * distance;
    }

    protected abstract void ChooseDirection();
}