using System;

namespace Framework.Utils.Math
{
    [Serializable]
    public class MinMaxValue
    {
        public float Min = float.MinValue;
        public float Max = float.MaxValue;

        public MinMaxValue()
        {
            Min = float.MinValue;
            Max = float.MaxValue;
        }

        public MinMaxValue(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float GetRandom()
        {
            return UnityEngine.Random.Range(Min, Max);
        }

        public float GetNormalized(float t)
        {
            return UnityEngine.Mathf.Lerp(Min, Max, t);
        }
    }
}