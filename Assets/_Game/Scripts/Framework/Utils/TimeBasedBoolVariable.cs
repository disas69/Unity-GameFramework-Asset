using UnityEngine;

namespace Framework.Utils
{
    public class TimeBasedBoolVariable
    {
        private readonly float _timeout;

        private bool _value;
        private float _time;

        public bool IsExpired { get; private set; }

        public bool Value
        {
            get => _value;
            set
            {
                _value = value;
                _time = Time.time;
            }
        }

        public TimeBasedBoolVariable(float timeout, bool value = false)
        {
            _value = value;
            _time = Time.time;
            _timeout = timeout;
        }

        public void Set(bool value)
        {
            Value = value;
            Update();
        }

        public void Update()
        {
            if (!_value)
            {
                IsExpired = true;
                return;
            }

            if (Time.time - _time >= _timeout)
            {
                _value = false;
                IsExpired = true;
            }
            else
            {
                IsExpired = false;
            }
        }
    }
}