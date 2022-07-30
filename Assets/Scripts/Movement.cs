using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private float _horizontalMovementVelocity = 2;
    [SerializeField] private float _jumpVelocity = 15;
    [SerializeField, Range(8, 32)] private const int _hitBufferSize = 16;
    [SerializeField] private const float _minMoveDistance = 0.01f;
    [SerializeField] private const float _shellRadius = 0.01f;

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _gravityModifier = 1f;
    [SerializeField] private float _minGroundNormalY = 0.5f;

    private bool _isGrounded;
    private ContactFilter2D _contactFilter2D;
    private RaycastHit2D[] _hitBuffer = new RaycastHit2D[_hitBufferSize];
    private List<RaycastHit2D> _hitList = new List<RaycastHit2D>(_hitBufferSize);
    private Vector2 _targetVelocity;
    private Vector2 _groundNormal;
    private Vector2 _velocity;

    public bool IsGrounded => _isGrounded;
    public Vector2 Velocity => _velocity;

    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _contactFilter2D.useTriggers = false;
        _contactFilter2D.SetLayerMask(_layerMask);
        _contactFilter2D.useLayerMask = true;
    }

    private void Update()
    {
        _targetVelocity = new Vector2(Input.GetAxis("Horizontal"), 0);

        Jump();
    }

    private void FixedUpdate()
    {
        _velocity += _gravityModifier * Physics2D.gravity * Time.deltaTime;
        _velocity.x = _targetVelocity.x * _horizontalMovementVelocity;

        _isGrounded = false;

        Vector2 deltaPosition = _velocity * Time.deltaTime;
        Vector2 movementAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
        Vector2 movement = movementAlongGround * deltaPosition.x;

        Move(movement, false);

        movement = Vector2.up * deltaPosition.y;

        Move(movement, true);

    }

    private void Jump ()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (_isGrounded)
            {
                _velocity.y = _jumpVelocity;
            }
        }
    }

    private void Move (Vector2 movement, bool yMovement)
    {
        float distance = movement.magnitude;

        if (distance > _minMoveDistance)
        {

            int count = _rigidbody.Cast(movement, _contactFilter2D, _hitBuffer, distance + _shellRadius);

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

                float modifiedDistance = _hitList[i].distance - _shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }

        _rigidbody.position = _rigidbody.position + movement.normalized * distance;
    }
}
