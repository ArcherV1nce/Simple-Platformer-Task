using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float _offsetZ;
    private Vector3 _lastTargetPosition;
    private Vector3 _currentVelocity;
    private Vector3 _lookAheadPos;

    [SerializeField] private Transform _target;
    [SerializeField] private float _damping = 1;
    [SerializeField] private float _lookAheadFactor = 3;
    [SerializeField] private float _lookAheadReturnSpeed = 0.5f;
    [SerializeField] private float _lookAheadMoveThreshold = 0.1f;

    private void Start()
    {
        _lastTargetPosition = _target.position;
        _offsetZ = (transform.position - _target.position).z;
        transform.parent = null;
    }


    private void FixedUpdate()
    {
        float xMoveDelta = (_target.position - _lastTargetPosition).x;

        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > _lookAheadMoveThreshold;

        if (updateLookAheadTarget)
        {
            _lookAheadPos = _lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
        }

        else
        {
            _lookAheadPos = Vector3.MoveTowards(_lookAheadPos, Vector3.zero, Time.deltaTime * _lookAheadReturnSpeed);
        }

        Vector3 aheadTargetPos = _target.position + _lookAheadPos + Vector3.forward * _offsetZ;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref _currentVelocity, _damping);

        transform.position = newPos;

        _lastTargetPosition = _target.position;
    }
}