using UnityEngine;

namespace Framework.Effects
{
    public class AnimationEffect : VisualEffect<Animation>
    {
        public override void Play()
        {
            base.Play();
            Effect.Play();
        }

        protected override bool IsPlaying()
        {
            return Effect.isPlaying;
        }
    }
}