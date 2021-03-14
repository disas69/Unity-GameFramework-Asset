using DG.Tweening;
using UnityEngine;

namespace Framework.Effects
{
    public class AlphaFadeAndScaleEffect : VisualEffect<Renderer>
    {
        private Sequence _sequence;
        private Color _defaultColor;
        private Vector3 _defaultScale;

        [SerializeField] private float _scale;
        [SerializeField] private float _duration;
        [SerializeField] private Ease _ease;

        public void SetMaterial(Material material)
        {
            Effect.material = material;
        }

        public override void Play()
        {
            base.Play();

            _defaultColor = Effect.material.color;
            _defaultScale = Effect.transform.localScale;

            _sequence = DOTween.Sequence()
                .Append(Effect.material.DOFade(0f, _duration))
                .Join(Effect.transform.DOScale(_scale, _duration))
                .SetEase(_ease)
                .Play();
        }

        protected override bool IsPlaying()
        {
            return _sequence.IsPlaying();
        }

        public override void Deactivate()
        {
            base.Deactivate();

            Effect.material.color = _defaultColor;
            Effect.transform.localScale = _defaultScale;

            if (_sequence != null)
            {
                _sequence.Kill();
            }
        }
    }
}
