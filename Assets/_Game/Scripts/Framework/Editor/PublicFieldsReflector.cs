using System;
using System.Reflection;

namespace Framework.Editor
{
    public class PublicFieldsReflector
    {
        private readonly FieldInfo[] _fields;

        public PublicFieldsReflector(Type type)
        {
            _fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        }

        public void ForEach(Action<FieldInfo> callback)
        {
            for (int i = 0; i < _fields.Length; i++)
            {
                callback?.Invoke(_fields[i]);
            }
        }
    }
}