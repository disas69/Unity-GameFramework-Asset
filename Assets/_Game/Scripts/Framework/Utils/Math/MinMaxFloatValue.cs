using System;

namespace Framework.Utils.Math
{
    [Serializable]
    public class MinMaxFloatValue
    {
        public float Min = float.MinValue;
        public float Max = float.MaxValue;

        public MinMaxFloatValue()
        {
            Min = float.MinValue;
            Max = float.MaxValue;
        }

        public MinMaxFloatValue(float min, float max)
        {
            Min = min;
            Max = max;
        }

        public float GetRandom()
        {
            return UnityEngine.Random.Range(Min, Max);
        }
    }
}