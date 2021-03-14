using UnityEngine;

namespace Framework.Tools.Misc
{
    [RequireComponent(typeof(Animation))]
    public class AnimationUscaledTimePlayer : MonoBehaviour
    {
        private float _time;
        private Animation _animation;
        private AnimationState _state;

        public bool RestartOnEnable = true;

        private void Awake()
        {
            _animation = GetComponent<Animation>();
            _state = _animation[_animation.clip.name];
        }

        private void OnEnable()
        {
            if (RestartOnEnable)
            {
                _time = 0;
            }
        }

        private void LateUpdate()
        {
            _state.time = _time += Time.unscaledDeltaTime;
            _animation.Sample();
        }
    }
}