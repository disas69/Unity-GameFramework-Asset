using System.Collections.Generic;
using Framework.Editor;
using Framework.Editor.GUIUtilities;
using Source.Configuration;
using Source.Data;
using Source.Level;
using UnityEditor;
using UnityEngine;

namespace Source.Editor
{
    [CustomEditor(typeof(GameConfiguration))]
    public class GameConfigurationEditor : CustomEditorBase<GameConfiguration>
    {
        private Vector2 _scrollPosition;

        protected override void DrawInspector()
        {
            base.DrawInspector();

            using (var scroll = new EditorGUILayout.ScrollViewScope(_scrollPosition))
            {
                _scrollPosition = scroll.scrollPosition;

                EditorGUILayout.LabelField("Game Configuration", HeaderStyle);

                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    DrawData();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    DrawSettings();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUI.skin.box);
                {
                    DrawLevels();
                }
                EditorGUILayout.EndVertical();
            }
        }

        private void DrawData()
        {
            EditorGUILayout.LabelField("Data", HeaderStyle);

            GameData.LevelIndex = EditorGUILayout.IntField("Level Index", GameData.LevelIndex);
            GameData.LevelProgression = EditorGUILayout.IntField("Level Progression", GameData.LevelProgression);
            GameData.IsTutorialComplete = EditorGUILayout.Toggle("Is Tutorial Complete", GameData.IsTutorialComplete);

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Save"))
                {
                    GameData.Save();
                }

                if (GUILayout.Button("Reset"))
                {
                    GameData.Reset();
                    GameData.Save();
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSettings()
        {
            EditorGUILayout.LabelField("Settings", HeaderStyle);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Game"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Player"), true);
        }

        private void DrawLevels()
        {
            EditorGUILayout.LabelField("Levels", HeaderStyle);
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Add"))
                {
                    RecordObject("Game Configuration Change");
                    Target.Levels.Add(new LevelConfiguration());
                }

                if (GUILayout.Button("Update"))
                {
                    var scenes = new List<EditorBuildSettingsScene>
                    {
                        new EditorBuildSettingsScene(GameEditor.MainScenePath, true)
                    };

                    for (var i = 0; i < Target.Levels.Count; i++)
                    {
                        var scenePath = Target.Levels[i].Scene;
                        if (!string.IsNullOrEmpty(scenePath))
                        {
                            scenes.Add(new EditorBuildSettingsScene(scenePath, true));
                        }
                    }

                    EditorBuildSettings.scenes = scenes.ToArray();
                    EditorApplication.ExecuteMenuItem("File/Save Project");
                }
            }
            EditorGUILayout.EndHorizontal();

            var points = serializedObject.FindProperty("Levels");
            var count = points.arraySize;
            for (var i = 0; i < count; i++)
            {
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                {
                    EditorGUILayout.BeginVertical();
                    {
                        var element = points.GetArrayElementAtIndex(i);
                        var level = element.FindPropertyRelative("Level");
                        level.intValue = i + 1;

                        using (new GUIEnabled(false))
                        {
                            EditorGUILayout.PropertyField(level);
                        }

                        var scene = element.FindPropertyRelative("Scene");
                        var sceneAsset = EditorGUILayout.ObjectField("Scene", AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.stringValue), typeof(SceneAsset), false);
                        scene.stringValue = AssetDatabase.GetAssetPath(sceneAsset);

                        EditorGUILayout.PropertyField(element.FindPropertyRelative("Settings"), true);
                    }
                    EditorGUILayout.EndVertical();

                    if (GUILayout.Button("X", GUILayout.Width(20)))
                    {
                        RecordObject("Game Configuration Change");
                        Target.Levels.RemoveAt(i);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}