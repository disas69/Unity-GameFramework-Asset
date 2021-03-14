using System.Collections;
using Framework.Extensions;
using UnityEngine;

namespace Framework.Audio
{
    [RequireComponent(typeof(AudioPlayer))]
    public class AudioEffects : MonoBehaviour
    {
        private AudioPlayer _audioPlayer;
        private Coroutine _fadeCoroutine;
        private Coroutine _pitchCoroutine;

        [SerializeField] private bool _fadeInOnAwake;
        [SerializeField] private float _fadeInTime = 1f;
        [SerializeField] private float _fadeOutTime = 1f;
        [SerializeField] private float _pitchUpTime = 1f;
        [SerializeField] private float _pitchDownTime = 1f;
        [SerializeField] private AnimationCurve _curve;

        private void Awake()
        {
            _audioPlayer = GetComponent<AudioPlayer>();

            if (_fadeInOnAwake)
            {
                Fade(false);
            }
        }

        public void Fade(bool fadeOut)
        {
            this.SafeStopCoroutine(_fadeCoroutine);
            _fadeCoroutine = StartCoroutine(fadeOut ? FadeOut() : FadeIn());
        }

        public void ChangePitch(bool pitchDown)
        {
            this.SafeStopCoroutine(_pitchCoroutine);
            _pitchCoroutine = StartCoroutine(pitchDown ? PitchDown() : PitchUp());
        }

        private IEnumerator FadeIn()
        {
            var time = 0f;
            var volume = _audioPlayer.Volume;

            while (time < _fadeInTime)
            {
                _audioPlayer.SetVolume(Mathf.Lerp(volume, 1f, _curve.Evaluate(time / _fadeInTime)));
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            _audioPlayer.SetVolume(1f);
            _fadeCoroutine = null;
        }

        private IEnumerator FadeOut()
        {
            var time = 0f;
            var volume = _audioPlayer.Volume;

            while (time < _fadeOutTime)
            {
                _audioPlayer.SetVolume(Mathf.Lerp(volume, 0f, _curve.Evaluate(time / _fadeOutTime)));
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            _audioPlayer.SetVolume(0f);
            _fadeCoroutine = null;
        }

        private IEnumerator PitchDown()
        {
            var time = 0f;
            var pitch = _audioPlayer.Pitch;

            while (time < _pitchDownTime)
            {
                _audioPlayer.SetPitch(Mathf.Lerp(pitch, 0f, _curve.Evaluate(time / _pitchDownTime)));
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            _audioPlayer.SetPitch(0f);
            _pitchCoroutine = null;
        }

        private IEnumerator PitchUp()
        {
            var time = 0f;
            var pitch = _audioPlayer.Pitch;

            while (time < _pitchUpTime)
            {
                _audioPlayer.SetPitch(Mathf.Lerp(pitch, 1f, _curve.Evaluate(time / _pitchUpTime)));
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            _audioPlayer.SetPitch(1f);
            _pitchCoroutine = null;
        }
    }
}