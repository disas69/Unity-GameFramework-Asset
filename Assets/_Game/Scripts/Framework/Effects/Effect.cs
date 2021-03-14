using System;
using Framework.Spawn;
using Framework.Tools.Data;
using UnityEngine;

namespace Framework.Effects
{
    [Serializable]
    public class Effect : StorageItem
    {
        [HideInInspector]
        public SpawnerSettings SpawnerSettings;
    }
}