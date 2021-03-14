using UnityEngine;

namespace Framework.Utils.Math
{
    public static class Vector3Extensions
    {
        public static Vector3 ClampX(this Vector3 vector, float min, float max)
        {
            return new Vector3(Mathf.Clamp(vector.x, min, max), vector.y, vector.z);
        }

        public static Vector3 ClampY(this Vector3 vector, float min, float max)
        {
            return new Vector3(vector.x, Mathf.Clamp(vector.y, min, max), vector.z);
        }

        public static Vector3 ClampZ(this Vector3 vector, float min, float max)
        {
            return new Vector3(vector.x, vector.y, Mathf.Clamp(vector.z, min, max));
        }

        public static Vector3 WithX(this Vector3 vector, float x)
        {
            return new Vector3(x, vector.y, vector.z);
        }

        public static Vector3 WithY(this Vector3 vector, float y)
        {
            return new Vector3(vector.x, y, vector.z);
        }

        public static Vector3 WithZ(this Vector3 vector, float z)
        {
            return new Vector3(vector.x, vector.y, z);
        }

        public static Vector3 RoundToTint(this Vector3 vector)
        {
            return new Vector3((int) vector.x, (int) vector.y, (int) vector.z);
        }
    }
}