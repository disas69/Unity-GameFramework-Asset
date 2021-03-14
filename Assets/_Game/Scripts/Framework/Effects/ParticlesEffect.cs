using UnityEngine;

namespace Framework.Effects
{
    public class ParticlesEffect : VisualEffect<ParticleSystem>
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