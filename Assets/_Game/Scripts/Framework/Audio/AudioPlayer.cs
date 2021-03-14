using System.Collections.Generic;
using Framework.Audio.Configuration;
using Framework.Tools.Gameplay;
using UnityEngine;

namespace Framework.Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        private List<AudioSource> _activeAudioSources;
        private Pool<AudioSource> _audioSourcePool;

        [SerializeField] private AudioStorage _audioStorage;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private int _audioPoolCapacity;
        [SerializeField] private int _clipCapacity = 5;
        [SerializeField] private float _volume = 1f;
        [SerializeField] private float _pitch = 1f;

        public float Volume => _volume;
        public float Pitch => _pitch;

        private void Awake()
        {
            _activeAudioSources = new List<AudioSource>();
            _audioSourcePool = new Pool<AudioSource>(_audioSource, transform, _audioPoolCapacity);
        }

        public void SetVolume(float volume)
        {
            _volume = volume;
        }

        public void SetPitch(float pitch)
        {
            _pitch = pitch;
        }

        public void Play(string audioKey)
        {
            var audioClip = _audioStorage.GetAudioClip(audioKey);
            if (audioClip != null)
            {
                Play(audioClip);
            }
        }

        public void Play(AudioClip audioClip)
        {
            if (GetActiveAudioSources(audioClip) < _clipCapacity)
            {
                var audioSource = _audioSourcePool.GetNext();
                audioSource.clip = audioClip;
                audioSource.volume = _volume;
                audioSource.pitch = _pitch;
                audioSource.Play();

                _activeAudioSources.Add(audioSource);
            }
        }

        public void Stop(string audioKey)
        {
            var audioClip = _audioStorage.GetAudioClip(audioKey);
            if (audioClip != null)
            {
                Stop(audioClip);
            }
        }

        public void Stop(AudioClip audioClip)
        {
            var audioSource = GetAudioSource(audioClip);
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }

        public void StopAll()
        {
            for (var i = _activeAudioSources.Count - 1; i >= 0; i--)
            {
                _activeAudioSources[i].Stop();
            }
        }

        public AudioSource GetAudioSource(AudioClip audioClip)
        {
            return _activeAudioSources.Find(a => a.clip == audioClip);
        }

        private void Update()
        {
            for (var i = _activeAudioSources.Count - 1; i >= 0; i--)
            {
                var audioSource = _activeAudioSources[i];
                audioSource.volume = _volume;
                audioSource.pitch = _pitch;

                if (!audioSource.isPlaying)
                {
                    Return(audioSource);
                    _activeAudioSources.RemoveAt(i);
                }
            }
        }

        private void Return(AudioSource audioSource)
        {
            audioSource.clip = null;
            audioSource.volume = 1f;
            audioSource.pitch = 1f;
            audioSource.loop = false;

            _audioSourcePool.Return(audioSource);
        }

        private int GetActiveAudioSources(AudioClip clip)
        {
            var count = 0;

            for (var i = 0; i < _activeAudioSources.Count; i++)
            {
                if (_activeAudioSources[i].clip == clip)
                {
                    count++;
                }
            }

            return count;
        }

        private void OnDestroy()
        {
            for (var i = 0; i < _activeAudioSources.Count; i++)
            {
                Return(_activeAudioSources[i]);
            }

            _activeAudioSources.Clear();
            _audioSourcePool.Dispose();
        }
    }
}