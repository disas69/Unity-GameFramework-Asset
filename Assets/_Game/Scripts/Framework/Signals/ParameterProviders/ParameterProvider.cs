using UnityEngine;

namespace Framework.Signals.ParameterProviders
{
    public abstract class ParameterProvider<T> : MonoBehaviour
    {
        public abstract T GetValue();
    }
}