using System;

namespace Framework.Utils.Math
{
    [Serializable]
    public class MinMaxIntValue
    {
        public int Min = int.MinValue;
        public int Max = int.MaxValue;

        public MinMaxIntValue()
        {
            Min = int.MinValue;
            Max = int.MaxValue;
        }

        public MinMaxIntValue(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public int GetRandom()
        {
            return UnityEngine.Random.Range(Min, Max + 1);
        }
    }
}