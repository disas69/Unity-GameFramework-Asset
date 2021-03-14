using DG.Tweening;
using UnityEngine;

namespace Framework.Animations
{
    public class ScaleInAnimation : AnimationBase
    {
        private Vector3 _scale;

        public override void Awake()
        {
            base.Awake();
            _scale = transform.localScale;
        }
        
        public override void Play()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(_scale, Time).SetEase(Ease);
        }

        public override void ResetAnimation()
        {
            transform.localScale = _scale;
        }
    }
}