using System;
using UnityEngine;
using Framework.Utils.Math;

namespace Framework.Input
{
    public class InputController : MonoBehaviour
    {
        private Vector2 _touchPosition;
        private Vector2 _lastTouchPosition;

        [SerializeField] private Vector2 _screenSize = Vector2.one;
        [SerializeField] private Vector2 _screenCenter = Vector2.one * 0.5f;
        [SerializeField] private bool _showScreenSize = false;
        [SerializeField] private bool _updateCenterX = false;
        [SerializeField] private bool _updateCenterY = false;

        public bool IsTouching { get; private set; }
        public Vector2 FrameDelta { get; private set; }
        public Vector2 Direction { get; private set; }
        public Vector2 NormalizedPosition { get; private set; }

        public event Action<Vector2> TouchDown;
        public event Action<Vector2> TouchUp;
        public event Action<Vector2> Drag;

        private void Update()
        {
            var touch = GetTouch();

            if (!IsTouching && touch)
            {
                IsTouching = true;

                _touchPosition = _lastTouchPosition = GetTouchPosition();

                if (_updateCenterX)
                {
                    _screenCenter.x = _touchPosition.x / Screen.width;
                }

                if (_updateCenterY)
                {
                    _screenCenter.y = _touchPosition.y / Screen.height;
                }

                NormalizedPosition = GetNormalizedPosition(_touchPosition);
                TouchDown?.Invoke(_touchPosition);
            }
            else if (IsTouching && !touch)
            {
                IsTouching = false;
                TouchUp?.Invoke(_lastTouchPosition);
                FrameDelta = Vector3.zero;
                _touchPosition = _lastTouchPosition = Vector3.zero;
                _screenCenter = Vector2.one * 0.5f;
                NormalizedPosition = Vector2.zero;
            }

            if (IsTouching)
            {
                var position = GetTouchPosition();
                NormalizedPosition = GetNormalizedPosition(position);

                var frameDelta = position - _lastTouchPosition;
                frameDelta.x /= Screen.width;
                frameDelta.y /= Screen.height;
                FrameDelta = frameDelta;

                _lastTouchPosition = position;

                Direction = (_lastTouchPosition - _touchPosition).normalized;
                Drag?.Invoke(_lastTouchPosition);
            }
        }

        private Vector2 GetNormalizedPosition(Vector2 position)
        {
            var difference = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f) - new Vector2(Screen.width * _screenCenter.x, Screen.height * _screenCenter.y);
            var screenPosition = position + difference;

            var width = Screen.width * _screenSize.x;
            var height = Screen.height * _screenSize.y;

            var x = (Screen.width - width) / 2f;
            var y = (Screen.height - height) / 2f;

            screenPosition.x = Mathf.Clamp(screenPosition.x - x, 0f, width);
            screenPosition.y = Mathf.Clamp(screenPosition.y - y, 0f, height);

            screenPosition.x /= width;
            screenPosition.y /= height;

            var normalizedX = MathUtility.Remap(screenPosition.x, 0f, 1f, -1f, 1f);
            var normalizedY = MathUtility.Remap(screenPosition.y, 0f, 1f, -1f, 1f);

            return new Vector2(normalizedX, normalizedY);
        }

        private bool GetTouch()
        {
#if UNITY_EDITOR
            return UnityEngine.Input.GetMouseButton(0);
#else
            return UnityEngine.Input.touchCount > 0;
#endif
        }

        private Vector2 GetTouchPosition()
        {
#if UNITY_EDITOR
            return UnityEngine.Input.mousePosition;
#else
            return UnityEngine.Input.GetTouch(0).position;
#endif
        }

#if UNITY_EDITOR
        private void OnGUI()
        {
            if (_showScreenSize)
            {
                var width = Screen.width * _screenSize.x;
                var height = Screen.height * _screenSize.y;

                var x = (Screen.width - width) / 2f;
                var y = (Screen.height - height) / 2f;

                GUI.Box(new Rect(x + (width * _screenCenter.x - width * 0.5f), y - (height * _screenCenter.y - height * 0.5f), width, height), "Screen size");
            }
        }
#endif
    }
}