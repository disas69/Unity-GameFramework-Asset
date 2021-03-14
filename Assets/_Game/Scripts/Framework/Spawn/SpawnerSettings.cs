using System;

namespace Framework.Spawn
{
    [Serializable]
    public class SpawnerSettings
    {
        public SpawnableObject ObjectPrefab;
        public int PoolCapacity;
    }
}