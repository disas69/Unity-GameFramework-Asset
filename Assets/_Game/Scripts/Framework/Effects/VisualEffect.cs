using UnityEngine;
using Framework.Spawn;
using UnityEngine.Events;

namespace Framework.Effects
{
    public interface IVisualEffect
    {
        Transform transform { get; }
        void Play();
        void Deactivate();
    }

    public abstract class VisualEffect<T> : SpawnableObject, IVisualEffect
    {
        private bool _isActive;

        [SerializeField] private T _effect;
        [SerializeField] private UnityEvent _onPlay;

        public T Effect => _effect;

        public virtual void Play()
        {
            _isActive = true;
            _onPlay.Invoke();
        }

        protected abstract bool IsPlaying();

        private void Update()
        {
            if (_isActive && !IsPlaying())
            {
                _isActive = false;
                Deactivate();
            }
        }
    }
}