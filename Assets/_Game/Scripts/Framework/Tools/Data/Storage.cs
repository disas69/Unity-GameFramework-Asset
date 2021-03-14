using System.Collections.Generic;
using UnityEngine;

namespace Framework.Tools.Data
{
    public class Storage<T> : ScriptableObject where T : StorageItem
    {
        public List<T> Items = new List<T>();

        public string[] GetNames()
        {
            var names = new string[Items.Count];

            for (var i = 0; i < Items.Count; i++)
            {
                names[i] = Items[i].Name;
            }

            return names;
        }
    }
}