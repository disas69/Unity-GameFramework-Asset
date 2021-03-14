using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework.Extensions;
using Framework.Utils.Debug;
using Framework.Utils.Math;
using Framework.Utils.Positioning;
using Source.Configuration;
using Source.Tools;
using UnityEngine;

namespace Source.Camera
{
    public enum CameraShakeType
    {
        None = 0,
        LightShake = 1,
        MediumShake = 2,
        HardShake = 3
    }

    [Serializable]
    public class AnchorTransform
    {
        public Vector3 Position;
        public Vector3 Rotation;
    }

    [RequireComponent(typeof(PositionFollower))]
    [RequireComponent(typeof(RotationFollower))]
    public class CameraController : ActivatableMonoBehaviour
    {
        private bool _isShaking;
        private bool _isInitialized;
        private Vector3 _defaultPosition;
        private Vector3 _defaultRotation;
        private PositionFollower _positionFollower;
        private RotationFollower _rotationFollower;

        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private Transform _cameraAnchor;
        [SerializeField] private bool _useCameraLimiters;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private LayerMask _wallsMask;
        [SerializeField] private float _focusTime;
        [SerializeField] private Ease _focusEase;
        [SerializeField] private List<AnchorTransform> _focusAnchors;

        public UnityEngine.Camera Camera => _camera;

        public void Initialize()
        {
            if (_isInitialized)
            {
                ResetState();
                return;
            }

            _positionFollower = GetComponent<PositionFollower>();
            _rotationFollower = GetComponent<RotationFollower>();
            _defaultPosition = _camera.transform.localPosition;
            _defaultRotation = _camera.transform.localRotation.eulerAngles;

            _isInitialized = true;
        }

        public override void Activate(bool isActive)
        {
            base.Activate(isActive);
            FollowPosition(isActive);
            FollowRotation(isActive);
        }

        public void FollowPosition(bool value)
        {
            _positionFollower.Activate(value);
        }

        public void FollowRotation(bool value)
        {
            _rotationFollower.Activate(value);
        }

        public void ApplyOffset(bool value)
        {
            _rotationFollower.ApplyForwardOffset(value);
        }

        public void Shake(int shakeType)
        {
            Shake((CameraShakeType)shakeType);
        }

        public void ResetState()
        {
            _isShaking = false;
            _camera.transform.DOKill();
            _cameraAnchor.transform.DOKill();
            _positionFollower.ResetState();
            _rotationFollower.SetRotationImmediately();
        }

        private void FixedUpdate()
        {
            if (!IsActive)
            {
                return;
            }

            UpdateCameraLimiters();
        }

        public void Focus(int index, bool immediately = false, Action callback = null)
        {
            if (index < _focusAnchors.Count)
            {
                var focusTransform = _focusAnchors[index];

                if (immediately)
                {
                    _cameraAnchor.transform.localPosition = focusTransform.Position;
                    _cameraAnchor.transform.localRotation = Quaternion.Euler(focusTransform.Rotation);

                    ResetCamera(true);
                    callback?.Invoke();
                }
                else
                {
                    _cameraAnchor.transform.DOKill();
                    _cameraAnchor.transform.DOLocalMove(focusTransform.Position, _focusTime).SetUpdate(UpdateType.Fixed).SetEase(_focusEase);
                    _cameraAnchor.transform.DOLocalRotate(focusTransform.Rotation, _focusTime).SetUpdate(UpdateType.Fixed).SetEase(_focusEase);

                    this.WaitForSeconds(_focusTime, () => callback?.Invoke());
                }
            }
        }

        public void UnFocus(Action callback = null)
        {
            _cameraAnchor.transform.DOKill();
            _cameraAnchor.transform.DOLocalMove(_defaultPosition, _focusTime).SetUpdate(UpdateType.Fixed).SetEase(_focusEase);
            _cameraAnchor.transform.DOLocalRotate(_defaultRotation, _focusTime).SetUpdate(UpdateType.Fixed).SetEase(_focusEase);

            this.WaitForSeconds(_focusTime, () => callback?.Invoke());
        }

        public bool IsVisible(GameObject target)
        {
            var screenPoint = _camera.WorldToViewportPoint(target.transform.position);
            if (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1)
            {
                return true;
            }

            return false;
        }

        private void Shake(CameraShakeType cameraShake, bool scaledTime = true)
        {
            switch (cameraShake)
            {
                case CameraShakeType.LightShake:
                    Shake(0.25f, 0.2f, 20, 90, true, !scaledTime);
                    break;
                case CameraShakeType.MediumShake:
                    Shake(0.5f, 0.4f, 25, 90, true, !scaledTime);
                    break;
                case CameraShakeType.HardShake:
                    Shake(1f, 0.7f, 30, 90, true, !scaledTime);
                    break;
            }
        }

        private void Shake(float duration, float strength, int vibrato, int randomness, bool fadeOut, bool unscaledTime)
        {
            if (_isShaking)
            {
                _camera.transform.DOKill();
            }

            _isShaking = true;
            _camera.transform.DOShakePosition(duration, strength, vibrato, randomness, false, fadeOut)
                .SetUpdate(UpdateType.Fixed)
                .SetUpdate(unscaledTime)
                .OnComplete(() =>
                {
                    _isShaking = false;
                    ResetCamera();
                });
        }

        private void ResetCamera(bool immediately = false)
        {
            if (immediately)
            {
                _camera.transform.localPosition = Vector3.zero;
                _camera.transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
            else
            {
                _camera.transform.DOLocalMove(Vector3.zero, 0.25f);
                _camera.transform.DOLocalRotate(Vector3.zero, 0.25f);
            }
        }

        private void UpdateCameraLimiters()
        {
            if (!_useCameraLimiters)
            {
                return;
            }

            var position = transform.position;
            position.z = 0f;

            var xMin = float.MinValue;
            var xMax = float.MaxValue;

            if (Physics.Raycast(position, Vector3.left, out var leftHit, 100f, _wallsMask))
            {
                Debug.DrawLine(position, leftHit.point, Color.white, 1f);
                var cameraLimiter = leftHit.collider.gameObject.GetComponent<CameraLimiter>();
                if (cameraLimiter != null && cameraLimiter.Type == CameraLimiterType.Horizontal)
                {
                    var point = leftHit.point;
                    DebugDrawer.DrawWireSphere(point, 1f, Color.green, 1f);
                    xMin = point.x - GameConfiguration.GameSettings.CameraLimiterOffset.x;
                }
            }

            if (Physics.Raycast(position, Vector3.right, out var rightHit, 100f, _wallsMask))
            {
                Debug.DrawLine(position, rightHit.point, Color.white, 1f);
                var cameraLimiter = rightHit.collider.gameObject.GetComponent<CameraLimiter>();
                if (cameraLimiter != null && cameraLimiter.Type == CameraLimiterType.Horizontal)
                {
                    var point = rightHit.point;
                    DebugDrawer.DrawWireSphere(point, 1f, Color.green, 1f);
                    xMax = point.x + GameConfiguration.GameSettings.CameraLimiterOffset.x;
                }
            }

            if (xMax < xMin)
            {
                xMax = xMin;
            }

            _positionFollower.SetupXMinMaxValue(new MinMaxFloatValue(xMin, xMax));

            var yMin = float.MinValue;
            var yMax = float.MaxValue;

            if (Physics.Raycast(position, Vector3.down, out var downHit, 100f, _groundMask))
            {
                Debug.DrawLine(position, downHit.point, Color.white, 1f);
                var cameraLimiter = downHit.collider.gameObject.GetComponent<CameraLimiter>();
                if (cameraLimiter != null && cameraLimiter.Type == CameraLimiterType.Vertical)
                {
                    var point = downHit.point;
                    DebugDrawer.DrawWireSphere(point, 1f, Color.green, 1f);
                    yMin = point.y - GameConfiguration.GameSettings.CameraLimiterOffset.y;
                }
            }

            if (Physics.Raycast(position, Vector3.up, out var upHit, 100f, _groundMask))
            {
                Debug.DrawLine(position, upHit.point, Color.white, 1f);
                var cameraLimiter = upHit.collider.gameObject.GetComponent<CameraLimiter>();
                if (cameraLimiter != null && cameraLimiter.Type == CameraLimiterType.Vertical)
                {
                    var point = upHit.point;
                    DebugDrawer.DrawWireSphere(point, 1f, Color.green, 1f);
                    yMax = point.y + GameConfiguration.GameSettings.CameraLimiterOffset.y;
                }
            }

            if (yMax < yMin)
            {
                yMax = yMin;
            }

            _positionFollower.SetupYMinMaxValue(new MinMaxFloatValue(yMin, yMax));
        }
    }
}