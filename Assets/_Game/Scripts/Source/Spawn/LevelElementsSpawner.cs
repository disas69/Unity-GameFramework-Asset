using System;
using Framework.Tools.Singleton;
using Framework.Spawn;
using UnityEngine;

namespace Source.Spawn
{
    public class LevelElementsSpawner : MonoSingleton<LevelElementsSpawner>, IDisposable
    {
        [SerializeField] private LevelElementsDictionary _dictionary;

        public static T Spawn<T>(LevelElementType levelElementType) where T : SpawnableObject
        {
            return Instance._dictionary[levelElementType].Spawn() as T;
        }

        public static void Clear()
        {
            foreach (var spawner in Instance._dictionary)
            {
                spawner.Value.Clear();
            }
        }

        public void Dispose()
        {
            foreach (var spawner in _dictionary)
            {
                spawner.Value.Dispose();
            }
        }
    }
}