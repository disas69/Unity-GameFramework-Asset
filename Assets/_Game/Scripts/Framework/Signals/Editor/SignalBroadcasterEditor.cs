using Framework.Signals.Broadcasters;
using UnityEditor;
using UnityEngine;

namespace Framework.Signals.Editor
{
    [CustomEditor(typeof(SignalBroadcaster<,>), true)]
    public class SignalBroadcasterEditor : UnityEditor.Editor
    {
        private GUIContent _icon;

        private void OnEnable()
        {
            _icon = EditorGUIUtility.IconContent("SettingsIcon");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();

            var isParameterProvided = serializedObject.FindProperty("IsParameterProvided");
            var parameter = serializedObject.FindProperty("Parameter");
            var parameterProvider = serializedObject.FindProperty("Provider");

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PropertyField(isParameterProvided.boolValue ? parameterProvider : parameter);
                isParameterProvided.boolValue = GUILayout.Toggle(isParameterProvided.boolValue, _icon, GUI.skin.button, GUILayout.Width(24f));
            }
            EditorGUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}