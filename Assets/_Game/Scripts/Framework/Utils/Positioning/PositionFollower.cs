using Framework.Utils.Math;
using UnityEngine;

namespace Framework.Utils.Positioning
{
    public class PositionFollower : MonoBehaviour
    {
        private bool _isActive;
        private Vector3 _offset;
        private Vector3 _velocity;
        private Vector3 _defaultPosition;
        private Vector3 _initialPosition;
        private Vector3 _lastCopiedPosition;

        [SerializeField] private Transform _target;
        [SerializeField] private bool _followX;
        [SerializeField] private bool _followY;
        [SerializeField] private bool _followZ;
        [SerializeField] private float _xDeadZone;
        [SerializeField] private float _yDeadZone;
        [SerializeField] private float _zDeadZone;
        [SerializeField] private MinMaxFloatValue _xMinMaxValue;
        [SerializeField] private MinMaxFloatValue _yMinMaxValue;
        [SerializeField] private MinMaxFloatValue _zMinMaxValue;
        [SerializeField] private float _moveSmoothing;

        private void Awake()
        {
            if (_target != null)
            {
                _isActive = true;
                _offset = transform.position - _target.transform.position;
                _defaultPosition = _initialPosition = _target.position + _offset;
            }
        }

        public void Activate(bool isActive)
        {
            _isActive = isActive;

            if (!isActive)
            {
                _velocity = Vector3.zero;
            }
        }

        public void SetTarget(Transform target, bool updateOffset)
        {
            _target = target;

            if (_target != null && updateOffset)
            {
                _offset = transform.position - _target.transform.position;
                _defaultPosition = _initialPosition = _target.position + _offset;
            }
        }

        public void Setup(bool followX, bool followY, bool followZ)
        {
            _followX = followX;
            _followY = followY;
            _followZ = followZ;
        }

        public void Setup(MinMaxFloatValue xMinMaxValue, MinMaxFloatValue yMinMaxValue, MinMaxFloatValue zMinMaxValue)
        {
            SetupXMinMaxValue(xMinMaxValue);
            SetupYMinMaxValue(yMinMaxValue);
            SetupZMinMaxValue(zMinMaxValue);
        }

        public void SetupXMinMaxValue(MinMaxFloatValue xMinMaxValue)
        {
            _xMinMaxValue = xMinMaxValue;
        }

        public void SetupYMinMaxValue(MinMaxFloatValue yMinMaxValue)
        {
            _yMinMaxValue = yMinMaxValue;
        }

        public void SetupZMinMaxValue(MinMaxFloatValue zMinMaxValue)
        {
            _zMinMaxValue = zMinMaxValue;
        }

        public void ResetState()
        {
            _velocity = Vector3.zero;
            _initialPosition = _defaultPosition;
            SetPositionImmediately();
        }

        public void SetPositionImmediately()
        {
            var targetPosition = GetTargetPosition();
            SetPosition(targetPosition, _followX, _followY, _followZ);
            _lastCopiedPosition = targetPosition;
        }

        private void FixedUpdate()
        {
            UpdatePosition();
        }

        public void UpdatePosition()
        {
            if (_isActive && _target != null)
            {
                var targetPosition = GetTargetPosition();

                if (_moveSmoothing > 0)
                {
                    var newPos = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _moveSmoothing,
                        float.PositiveInfinity, Time.unscaledDeltaTime);
                    SetPosition(newPos, _followX, _followY, _followZ);
                }
                else if (targetPosition != _lastCopiedPosition)
                {
                    SetPosition(targetPosition, _followX, _followY, _followZ);
                    _lastCopiedPosition = targetPosition;
                }
            }
        }

        public Vector3 GetTargetPosition()
        {
            if (_target != null)
            {
                var targetPosition = _target.position + _offset;

                if (Mathf.Abs(_initialPosition.x - targetPosition.x) > _xDeadZone)
                {
                    if (_initialPosition.x > targetPosition.x)
                    {
                        _initialPosition.x -= _xDeadZone;
                    }
                    else
                    {
                        _initialPosition.x += _xDeadZone;
                    }
                }
                else
                {
                    targetPosition.x = _initialPosition.x;
                }

                if (Mathf.Abs(_initialPosition.y - targetPosition.y) > _yDeadZone)
                {
                    if (_initialPosition.y > targetPosition.y)
                    {
                        _initialPosition.y -= _yDeadZone;
                    }
                    else
                    {
                        _initialPosition.y += _yDeadZone;
                    }
                }
                else
                {
                    targetPosition.y = _initialPosition.y;
                }

                if (Mathf.Abs(_initialPosition.z - targetPosition.z) > _zDeadZone)
                {
                    if (_initialPosition.z > targetPosition.z)
                    {
                        _initialPosition.z -= _zDeadZone;
                    }
                    else
                    {
                        _initialPosition.z += _zDeadZone;
                    }
                }
                else
                {
                    targetPosition.z = _initialPosition.z;
                }

                return targetPosition;
            }

            return Vector3.zero;
        }

        private void SetPosition(Vector3 position, bool followX, bool followY, bool followZ)
        {
            var newPosition = transform.position;

            if (followX)
            {
                newPosition.x = Mathf.Clamp(position.x, _xMinMaxValue.Min, _xMinMaxValue.Max);
            }

            if (followY)
            {
                newPosition.y = Mathf.Clamp(position.y, _yMinMaxValue.Min, _yMinMaxValue.Max);
            }

            if (followZ)
            {
                newPosition.z = Mathf.Clamp(position.z, _zMinMaxValue.Min, _zMinMaxValue.Max);
            }

            transform.position = newPosition;
        }
    }
}