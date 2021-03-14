using Framework.Editor;
using UnityEditor;

namespace Framework.Effects.Editor
{
    [CustomEditor(typeof(EffectsStorage))]
    public class EffectsStorageEditor : StorageEditor<Effect, EffectsStorage>
    {
        protected override void DrawItem(SerializedProperty itemName, SerializedProperty item, int index)
        {
            base.DrawItem(itemName, item, index);
            var settings = item.FindPropertyRelative("SpawnerSettings");
            EditorGUILayout.PropertyField(settings.FindPropertyRelative("ObjectPrefab"));
            EditorGUILayout.PropertyField(settings.FindPropertyRelative("PoolCapacity"));
        }
    }
}