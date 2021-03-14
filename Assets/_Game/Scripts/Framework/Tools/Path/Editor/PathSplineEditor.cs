using System.Reflection;
using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Framework.Tools.Path.Editor
{
    [CustomEditor(typeof(PathSpline))]
    public class PathSplineEditor : CustomEditorBase<PathSpline>
    {
        protected override void DrawInspector()
        {
            base.DrawInspector();
            
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Points", HeaderStyle);

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Add"))
                    {
                        RecordObject("Path Spline Change");
                        var point = new GameObject("Point " + (Target.Points.Count + 1));
                        AssignIcon(point);
                        point.transform.SetParent(Target.transform);
                        Target.Points.Add(point.transform);
                        Selection.activeObject = point;
                    }

                    if (GUILayout.Button("Build"))
                    {
                        Target.Build();
                    }
                }
                EditorGUILayout.EndHorizontal();
                
                var points = serializedObject.FindProperty("Points");
                var count = points.arraySize;
                for (int i = 0; i < count; i++)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        var element = points.GetArrayElementAtIndex(i);
                        EditorGUILayout.PropertyField(element, new GUIContent("Point " + (i + 1)));

                        if (GUILayout.Button("X", GUILayout.Width(20)))
                        {
                            RecordObject("Path Spline Change");
                            DestroyImmediate(Target.transform.GetChild(i).gameObject);
                            Target.Points.RemoveAt(i);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }

        private static void AssignIcon(GameObject gameObject)
        {
            var tex = EditorGUIUtility.IconContent("sv_label_0").image as Texture2D;
            var editorGUIUtilityType  = typeof(EditorGUIUtility);
            var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
            var args = new object[] {gameObject, tex};
            editorGUIUtilityType.InvokeMember("SetIconForObject", bindingFlags, null, null, args);
        }
    }
}