using System;
using Framework.DataStructures;
using Framework.Spawn;

namespace Source.Spawn
{
    [Serializable]
    public class LevelElementsDictionary : SerializableDictionary<LevelElementType, Spawner>
    {
    }
}