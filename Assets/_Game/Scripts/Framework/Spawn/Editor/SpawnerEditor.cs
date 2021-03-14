using Framework.Editor;
using Framework.Editor.GUIUtilities;
using UnityEditor;
using UnityEngine;

namespace Framework.Spawn.Editor
{
    [CustomEditor(typeof(Spawner))]
    public class SpawnerEditor : CustomEditorBase<Spawner>
    {
        protected override void DrawInspector()
        {
            base.DrawInspector();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Spawn Settings", HeaderStyle);
                
                var activateOnAwake = serializedObject.FindProperty("_activateOnAwake");
                EditorGUILayout.PropertyField(activateOnAwake);
                
                var spawnOnDestroySettings = serializedObject.FindProperty("_settings");
                var objectPrefab = spawnOnDestroySettings.FindPropertyRelative("ObjectPrefab");
                var poolCapacity = spawnOnDestroySettings.FindPropertyRelative("PoolCapacity");

                EditorGUILayout.PropertyField(objectPrefab);
                EditorGUILayout.PropertyField(poolCapacity);

                if (EditorApplication.isPlaying)
                {
                    using (new GUIEnabled(false))
                    {
                        EditorGUILayout.LabelField($"Active objects: {Target.Count}");
                        EditorGUILayout.LabelField($"Max Active objects: {Target.MaxCount}");
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}