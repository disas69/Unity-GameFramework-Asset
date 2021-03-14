using UnityEngine;

namespace Framework.Tools.Gameplay
{
    public class Spline
    {
        private Vector3[] _points;
        private float[] _tParams;
        private float _length;

        public Vector3[] Points => _points;
        public float Length => _length;

        public Spline(Vector3[] innerPoints, bool closedForm = false)
        {
            BuildSpline(innerPoints, closedForm);
        }

        public void BuildSpline(Vector3[] innerPoints, bool closedForm = false)
        {
            if (innerPoints.Length < 2)
            {
                Debug.LogWarning("invalid points");
                return;
            }

            var size = closedForm ? innerPoints.Length + 3 : innerPoints.Length + 2;
            _points = new Vector3[size];

            for (var i = 0; i < innerPoints.Length; i++) _points[i + 1] = innerPoints[i];

            var lastIndex = _points.Length - 1;
            if (closedForm)
            {
                _points[lastIndex - 1] = _points[1];
                _points[0] = _points[lastIndex - 2];
                _points[lastIndex] = _points[2];
            }
            else
            {
                _points[0] = _points[1] + (_points[1] - _points[2]);
                _points[lastIndex] = _points[lastIndex - 1] + (_points[lastIndex - 1] - _points[lastIndex - 2]);
            }

            _tParams = new float[_points.Length];
            _tParams[0] = 0f;

            for (var i = 1; i < _tParams.Length; i++)
            {
                _tParams[i] = _tParams[i - 1] + ComputeTParamDelta(_points[i - 1], _points[i]);
            }

            _length = _tParams[_tParams.Length - 2] - _tParams[1];
        }

        public Vector3 Interpolate(float value)
        {
            var clampedValue = Mathf.Clamp(value, 0, 1);
            var tValue = clampedValue * _length;
            return Evaluate(_tParams[1] + tValue);
        }

        private Vector3 Evaluate(float t)
        {
            if (t < _tParams[1] || t > _tParams[_tParams.Length - 2])
            {
                Debug.LogWarning("Points within the control vectors cannot be evaluated");
                return Vector3.zero;
            }

            var firstIndex = -1;
            //first, we need to find the 4 tParams surrounding our input
            for (var i = 2; i < _tParams.Length - 1; i++)
                if (t <= _tParams[i])
                {
                    firstIndex = i - 2;
                    break;
                }

            if (firstIndex == -1)
            {
                Debug.LogWarning("Could not compute tParams for evaluation");
                return Vector3.zero;
            }

            var t0 = _tParams[firstIndex];
            var t1 = _tParams[firstIndex + 1];
            var t2 = _tParams[firstIndex + 2];
            var t3 = _tParams[firstIndex + 3];

            var p0 = _points[firstIndex];
            var p1 = _points[firstIndex + 1];
            var p2 = _points[firstIndex + 2];
            var p3 = _points[firstIndex + 3];

            var a1 = (t1 - t) / (t1 - t0) * p0 + (t - t0) / (t1 - t0) * p1;
            var a2 = (t2 - t) / (t2 - t1) * p1 + (t - t1) / (t2 - t1) * p2;
            var a3 = (t3 - t) / (t3 - t2) * p2 + (t - t2) / (t3 - t2) * p3;

            var b1 = (t2 - t) / (t2 - t0) * a1 + (t - t0) / (t2 - t0) * a2;
            var b2 = (t3 - t) / (t3 - t1) * a2 + (t - t1) / (t3 - t1) * a3;

            var c = (t2 - t) / (t2 - t1) * b1 + (t - t1) / (t2 - t1) * b2;

            return c;
        }

        private float ComputeTParamDelta(Vector3 p0, Vector3 p1)
        {
            return Vector3.Distance(p0, p1);
        }
    }
}