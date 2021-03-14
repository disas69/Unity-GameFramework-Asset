using Framework.Tools.Misc;
using UnityEditor;
using UnityEngine;

namespace Framework.Editor
{
    [CustomEditor(typeof(GraphicsSettingsSetter))]
    public class GraphicsSettingsSetterEditor : CustomEditorBase<GraphicsSettingsSetter>
    {
        protected override void DrawInspector()
        {
            base.DrawInspector();

            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("FrameRate"));
                
                var qualityLevel = serializedObject.FindProperty("QualityLevel");
                qualityLevel.intValue = EditorGUILayout.Popup("Quality", qualityLevel.intValue, QualitySettings.names);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("VSync"));
            }
        }
    }
}
