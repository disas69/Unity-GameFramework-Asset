using System;
using System.Collections.Generic;
using Framework.Spawn;
using Framework.Tools.Singleton;
using UnityEngine;

namespace Framework.Effects
{
    [Serializable]
    public class EffectSpawnConfig
    {
        public string Effect;
        public Spawner Spawner;
    }

    public class VisualEffectsManager : MonoSingleton<VisualEffectsManager>
    {
        private readonly List<EffectSpawnConfig> _effects = new List<EffectSpawnConfig>();

        [SerializeField] private EffectsStorage _effectsStorage;

        protected override void Awake()
        {
            base.Awake();

            for (var i = 0; i < _effectsStorage.Items.Count; i++)
            {
                var effect = _effectsStorage.Items[i];
                var spawner = new GameObject($"Spawner [{effect.Name}]").AddComponent<Spawner>();
                spawner.transform.SetParent(transform);
                spawner.Activate(effect.SpawnerSettings);

                _effects.Add(new EffectSpawnConfig {Effect = effect.Name, Spawner = spawner});
            }
        }

        public static void Play(string effectName, Vector3 position, Quaternion rotation, Action<IVisualEffect> callback = null)
        {
            var spawnConfig = Instance._effects.Find(c => c.Effect == effectName);
            if (spawnConfig != null)
            {
                var effect = spawnConfig.Spawner.Spawn() as IVisualEffect;
                if (effect != null)
                {
                    effect.transform.SetParent(Instance.transform);
                    effect.transform.position = position;
                    effect.transform.rotation = rotation;
                    callback?.Invoke(effect);
                    effect.Play();
                }
            }
            else
            {
                Debug.LogError($"Failed to find effect config by name {effectName}");
            }
        }

        public static void Clear()
        {
            for (var i = 0; i < Instance._effects.Count; i++)
            {
                Instance._effects[i].Spawner.Clear();
            }
        }
    }
}