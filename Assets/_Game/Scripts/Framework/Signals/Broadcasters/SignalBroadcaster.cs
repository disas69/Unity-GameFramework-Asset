using Framework.Signals.ParameterProviders;
using UnityEngine;

namespace Framework.Signals.Broadcasters
{
    public interface ISignalBroadcaster
    {
        string SignalName { get; }
        GameObject GameObject { get; }
    }
    
    public class SignalBroadcaster : MonoBehaviour, ISignalBroadcaster
    {
        public Signal Signal;
        
        public string SignalName => Signal.Name;
        public GameObject GameObject => gameObject;

        public void Broadcast()
        {
            SignalsManager.Broadcast(Signal.Name);
        }
    }

    public abstract class SignalBroadcaster<TParameter, TParameterProvider> : MonoBehaviour, ISignalBroadcaster
        where TParameterProvider : ParameterProvider<TParameter>
    {
        public Signal Signal;
        [HideInInspector] public bool IsParameterProvided;
        [HideInInspector] public TParameter Parameter;
        [HideInInspector] public TParameterProvider Provider;
        
        public string SignalName => Signal.Name;
        public GameObject GameObject => gameObject;

        public abstract void Broadcast();
        public abstract void Broadcast(TParameter parameter);
        public abstract void Broadcast(TParameterProvider parameterProvider);
    }
}