using Framework.Audio.Configuration;
using Framework.Editor;
using UnityEditor;

namespace Framework.Audio.Editor
{
    [CustomEditor(typeof(AudioStorage))]
    public class AudioStorageEditor : StorageEditor<Audio, AudioStorage>
    {
        protected override void DrawItem(SerializedProperty itemName, SerializedProperty item, int index)
        {
            base.DrawItem(itemName, item, index);
            EditorGUILayout.PropertyField(item.FindPropertyRelative("AudioClip"));
        }
    }
}