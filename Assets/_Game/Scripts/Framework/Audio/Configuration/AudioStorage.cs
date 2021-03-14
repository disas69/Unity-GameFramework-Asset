using System;
using System.Collections.Generic;
using Framework.Tools.Data;
using UnityEngine;

namespace Framework.Audio.Configuration
{
    [CreateAssetMenu(fileName = "AudioStorage", menuName = "Stotage/AudioStorage")]
    public class AudioStorage : Storage<Audio>
    {
        private Dictionary<string, AudioClip> _audioDictionary = new Dictionary<string, AudioClip>();
        [NonSerialized] private bool _initialized;

        public AudioClip GetAudioClip(string key)
        {
            if (!_initialized)
            {
                for (var i = 0; i < Items.Count; i++)
                {
                    var config = Items[i];
                    _audioDictionary.Add(config.Name, config.AudioClip);
                }

                _initialized = true;
            }

            AudioClip audioClip;
            if (_audioDictionary.TryGetValue(key, out audioClip))
            {
                return audioClip;
            }

            Debug.LogError($"Failed to find Audio Clip by key {key}");
            return null;
        }
    }
}