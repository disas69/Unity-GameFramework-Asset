using UnityEngine;

namespace Framework.Utils.Math
{
    public static class MathUtility
    {
        public static float Remap(float value, float min1, float max1, float min2, float max2)
        {
            return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
        }

        public static float GetAngle(Vector3 lhs, Vector3 rhs)
        {
            var cosAngle = Vector3.Dot(lhs, rhs);
            if (cosAngle >= 1f)
            {
                return 0f;
            }

            if (cosAngle <= -1f)
            {
                return 180f;
            }

            var angle = Mathf.Acos(cosAngle) * Mathf.Rad2Deg;
            return angle;
        }

        public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            var direction = point - pivot;
            direction = Quaternion.Euler(angles) * direction;
            point = direction + pivot;
            return point;
        }

        public static Vector3 GetBallisticVelocity(Vector3 position, Vector3 destination, float angle)
        {
            var direction = destination - position;
            var height = direction.y;
            direction.y = 0;
            var distance = direction.magnitude;
            var rad = angle * Mathf.Deg2Rad;
            direction.y = distance * Mathf.Tan(rad);
            distance += height / Mathf.Tan(rad);

            var velocity = Mathf.Sqrt(distance * UnityEngine.Physics.gravity.magnitude / Mathf.Sin(2 * rad));
            if (float.IsNaN(velocity))
            {
                velocity = 1f;
            }

            return velocity * direction.normalized;
        }
    }
}