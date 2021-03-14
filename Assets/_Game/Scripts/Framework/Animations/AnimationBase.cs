using DG.Tweening;
using UnityEngine;

namespace Framework.Animations
{
    public abstract class AnimationBase : MonoBehaviour
    {
        [SerializeField] private bool _playOnStart;
        [SerializeField] private bool _playOnEnable;
        [SerializeField] private float _time;
        [SerializeField] private Ease _ease;

        public float Time => _time;
        public Ease Ease => _ease;

        public virtual void Awake()
        {
        }

        public virtual void Start()
        {
            if (_playOnStart)
            {
                Play();
            }
        }

        public virtual void OnEnable()
        {
            if (_playOnEnable)
            {
                Play();
            }
        }

        public virtual void OnDisable()
        {
            ResetAnimation();
        }

        public abstract void Play();
        public abstract void ResetAnimation();
    }
}