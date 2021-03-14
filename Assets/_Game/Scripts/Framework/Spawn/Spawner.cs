using System;
using System.Collections.Generic;
using Framework.Tools.Gameplay;
using UnityEngine;

namespace Framework.Spawn
{
    [Serializable]
    public class Spawner : MonoBehaviour, IDisposable
    {
        private int _maxCount;
        private Pool<SpawnableObject> _objectsPool;
        private List<SpawnableObject> _activeObjects;

        [SerializeField] private bool _activateOnAwake = true;
        [SerializeField] private SpawnerSettings _settings;
        
        public int Count => _activeObjects.Count;
        public int MaxCount => _maxCount;
        public List<SpawnableObject> ActiveObjects => _activeObjects;

        private void Awake()
        {
            _activeObjects = new List<SpawnableObject>();

            if (_activateOnAwake && _settings != null)
            {
                Activate(_settings);
            }
        }

        public void Activate(SpawnerSettings spawnerSettings)
        {
            if (_objectsPool == null)
            {
                _settings = spawnerSettings;
                _objectsPool = new Pool<SpawnableObject>(spawnerSettings.ObjectPrefab, transform, spawnerSettings.PoolCapacity);
            }
        }

        public SpawnableObject Spawn()
        {
            var spawnableObject = _objectsPool.GetNext();
            if (spawnableObject != null)
            {
                spawnableObject.Deactivated += OnObjectDeactivated;
                _activeObjects.Add(spawnableObject);

                if (_activeObjects.Count > _maxCount)
                {
                    _maxCount = _activeObjects.Count;
                }
            }
            
            return spawnableObject;
        }

        public void Clear()
        {
            for (var i = _activeObjects.Count - 1; i >= 0; i--)
            {
                _activeObjects[i].Deactivate();
            }
        }

        public void Dispose()
        {
            _objectsPool.Dispose();
        }

        private void OnObjectDeactivated(SpawnableObject spawnableObject)
        {
            Despawn(spawnableObject);
        }

        private void Despawn(SpawnableObject spawnableObject)
        {
            spawnableObject.Deactivated -= OnObjectDeactivated;

            _activeObjects.Remove(spawnableObject);
            _objectsPool.Return(spawnableObject);
        }
    }
}