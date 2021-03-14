using UnityEngine;
using UnityEngine.Events;

namespace Framework.Signals.Listeners
{
    public interface ISignalListener
    {
        string SignalName { get; }
        string[] Actions { get; }
        GameObject GameObject { get; }
    }
    
    public class SignalListener : MonoBehaviour, ISignalListener
    {
        public Signal Signal;
        public UnityEvent Action;

        public string SignalName => Signal.Name;
        public string[] Actions => GetActions();

        public GameObject GameObject => gameObject;

        private void OnEnable()
        {
            SignalsManager.Register(Signal.Name, Action.Invoke);
        }

        private void OnDisable()
        {
            SignalsManager.Unregister(Signal.Name, Action.Invoke);
        }

        private string[] GetActions()
        {
            var count = Action.GetPersistentEventCount();
            var actions = new string[count];

            for (var i = 0; i < count; i++)
            {
                actions[i] = Action.GetPersistentMethodName(i);
            }

            return actions;
        }
    }

    public abstract class SignalListener<T, TEvent> : MonoBehaviour, ISignalListener where TEvent : UnityEvent<T>, new()
    {
        public Signal Signal;
        public TEvent Action;
        
        public string SignalName => Signal.Name;
        public string[] Actions => GetActions();
        public GameObject GameObject => gameObject;

        protected abstract void Register();
        protected abstract void Unregister();

        private void OnEnable()
        {
            Register();
        }

        private void OnDisable()
        {
            Unregister();
        }
        
        private string[] GetActions()
        {
            var count = Action.GetPersistentEventCount();
            var actions = new string[count];

            for (var i = 0; i < count; i++)
            {
                actions[i] = Action.GetPersistentMethodName(i);
            }

            return actions;
        }
        
    }
}