using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class Movement : MonoBehaviour
{
    [SerializeField] protected ContactFilter2D _contactFilter2D;
    [SerializeField] protected LayerMask _layerMask;
    [SerializeField] protected float _horizontalMovementVelocity = 5;
    [SerializeField] protected float _jumpVelocity = 15;
    [SerializeField] protected float _gravityModifier = 2f;
    [SerializeField] protected float _minGroundNormalY = 0.5f;

    private const int HitBufferSize = 16;
    private const float MinMoveDistance = 0.01f;
    private const float ShellRadius = 0.01f;

    private Rigidbody2D _rigidbody;
    private Collider2D _collider;
    private bool _isGrounded;
    private readonly RaycastHit2D[] _hitBuffer = new RaycastHit2D[HitBufferSize];
    private readonly List<RaycastHit2D> _hitList = new(HitBufferSize);
    protected Vector2 _targetVelocity;
    private Vector2 _groundNormal;
    private Vector2 _velocity;

    public bool IsGrounded => _isGrounded;
    public Vector2 Velocity => _velocity;

    private void OnEnable()
    {
        SetUp();
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

    protected virtual void Move()
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
                if (_hitBuffer[i].collider.isTrigger == false)
                {
                    if (CheckForPlatform(_hitBuffer[i]))
                    {
                        continue;
                    }

                    _hitList.Add(_hitBuffer[i]);
                }
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
                    _velocity -= projection * currentNormal;
                }

                float modifiedDistance = _hitList[i].distance - ShellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        _rigidbody.position += movement.normalized * distance;
    }

    private bool CheckForPlatform(RaycastHit2D hit)
    {
        if (hit.collider.TryGetComponent(out PlatformEffector2D platform) &&
                        platform.transform.position.y > transform.position.y - _collider.bounds.size.y)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private void SetUp()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _contactFilter2D.SetLayerMask(_layerMask);
        _contactFilter2D.useLayerMask = true;
        _contactFilter2D.useTriggers = false;
    }

    protected abstract void ChooseDirection();
}