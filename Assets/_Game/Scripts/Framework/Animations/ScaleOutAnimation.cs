using DG.Tweening;
using UnityEngine;

namespace Framework.Animations
{
    public class ScaleOutAnimation : AnimationBase
    {
        private Vector3 _scale;

        public override void Awake()
        {
            base.Awake();
            _scale = transform.localScale;
        }

        public override void Play()
        {
            transform.DOScale(Vector3.zero, Time).SetEase(Ease);
        }

        public override void ResetAnimation()
        {
            transform.localScale = _scale;
        }
    }
}