using System.Collections.Generic;
using UnityEngine;

namespace Framework.Utils
{
    public static class ListUtilities
    {
        public static void Shuffle<T>(IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                var k = Random.Range(0, n) % n;
                n--;
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            var tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }


        public static void ShiftLeft<T>(IList<T> lst, int shifts)
        {
            for (var i = shifts; i < lst.Count; i++)
            {
                lst[i - shifts] = lst[i];
            }

            for (var i = lst.Count - shifts; i < lst.Count; i++)
            {
                lst[i] = default(T);
            }
        }

        public static void ShiftRight<T>(IList<T> lst, int shifts)
        {
            for (var i = lst.Count - shifts - 1; i >= 0; i--)
            {
                lst[i + shifts] = lst[i];
            }

            for (var i = 0; i < shifts; i++)
            {
                lst[i] = default(T);
            }
        }
    }
}