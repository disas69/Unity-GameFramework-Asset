using Framework.DataStructures.Editor;
using Framework.Spawn;
using UnityEditor;

namespace Source.Spawn.Editor
{
    [CustomPropertyDrawer(typeof(LevelElementsDictionary))]
    public class LevelElementsDictionaryEditor : SerializableDictionaryEditor<LevelElementType, Spawner>
    {
    }
}