using UnityEngine;

namespace Framework.Utils.Positioning
{
    public class RotationFollower : MonoBehaviour
    {
        private bool _isActive;

        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private bool _applyOffset;
        [SerializeField] private bool _followX;
        [SerializeField] private bool _followY;
        [SerializeField] private bool _followZ;
        [SerializeField] private float _smoothTime;

        public void Activate(bool isActive)
        {
            _isActive = isActive;
        }

        public void ApplyForwardOffset(bool value)
        {
            _applyOffset = value;
        }

        public void SetRotationImmediately()
        {
            var targetRotation = GetRotation(transform.rotation.eulerAngles, _applyOffset);
            transform.rotation = Quaternion.Euler(targetRotation);
        }

        private void FixedUpdate()
        {
            if (_isActive)
            {
                UpdateRotation();
            }
        }

        private void UpdateRotation()
        {
            if (_target != null)
            {
                var currentRotation = transform.rotation.eulerAngles;
                var targetRotation = GetRotation(currentRotation, _applyOffset);

                if (_smoothTime > 0f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRotation), _smoothTime);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(targetRotation);
                }
            }
        }

        private Vector3 GetRotation(Vector3 currentRotation, bool withOffset)
        {
            var targetRotation = currentRotation;
            var forward = withOffset
                ? (_target.transform.position + _target.transform.forward * _offset.y) - transform.position
                : (_target.transform.position - transform.position);
            var lookRotation = Quaternion.LookRotation(forward, Vector3.up).eulerAngles;

            if (_followX)
            {
                targetRotation.x = lookRotation.x;
            }

            if (_followY)
            {
                targetRotation.y = lookRotation.y;
            }

            if (_followZ)
            {
                targetRotation.z = lookRotation.z;
            }

            return targetRotation;
        }
    }
}