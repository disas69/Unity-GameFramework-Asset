using Framework.Extensions;
using UnityEngine;

namespace Framework.Effects
{
    public class GameObjectEffect : VisualEffect<GameObject>
    {
        private bool _playing;

        public float Time;

        public override void Play()
        {
            base.Play();

            _playing = true;
            Effect.SetActive(true);

            this.WaitForSeconds(Time, () =>
            {
                _playing = false;
                Effect.SetActive(false);
            });
        }

        protected override bool IsPlaying()
        {
            return _playing;
        }
    }
}