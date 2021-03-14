using UnityEngine;
using UnityEngine.EventSystems;

namespace Framework.Input
{
    public enum TouchResult
    {
        None,
        Tap,
        Swipe,
        Hold
    }

    public class TapDetector
    {
        private readonly float _distanceLimit;
        private readonly float _timeLimit;

        private bool _isActive;
        private float _touchDownTime;
        private Vector2 _touchDownPosition;

        public TapDetector(float distanceLimit = 0.25f, float timeLimit = 0.25f)
        {
            _distanceLimit = distanceLimit;
            _timeLimit = timeLimit;
        }

        public void RegisterTouchDown(Vector2 position)
        {
            _isActive = true;
            _touchDownTime = Time.time;
            _touchDownPosition = position;
        }

        public TouchResult Update(Vector2 position)
        {
            if (_isActive)
            {
                var distance = Vector2.Distance(_touchDownPosition, position);
                var deltaInInches = distance / Screen.dpi;
                if (deltaInInches > _distanceLimit)
                {
                    _isActive = false;
                    return TouchResult.Swipe;
                }
            }

            return TouchResult.None;
        }

        public TouchResult Update()
        {
            if (_isActive)
            {
                if (Time.time - _touchDownTime > _timeLimit)
                {
                    _isActive = false;
                    return TouchResult.Hold;
                }
            }

            return TouchResult.None;
        }

        public TouchResult RegisterTouchUp(Vector2 position)
        {
            if (_isActive)
            {
                if (Time.time - _touchDownTime < _timeLimit)
                {
                    var distance = Vector2.Distance(_touchDownPosition, position);
                    var deltaInInches = distance / Screen.dpi;
                    if (deltaInInches < _distanceLimit)
                    {
                        _isActive = false;
                        return TouchResult.Tap;
                    }

                    _isActive = false;
                    return TouchResult.Swipe;
                }
            }

            _isActive = false;
            return TouchResult.None;
        }
    }
}