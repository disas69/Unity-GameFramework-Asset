using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Editor;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Source.Editor
{
    public class GameSetupWindow : EditorWindow
    {
        private GUIStyle _headerStyle;
        private GUIStyle _labelStyle;
        private Vector2 _scrollPosition;
        private UIOrientation _orientation = UIOrientation.Portrait;
        private bool _showSplashScreen = false;
        private string _companyName;
        private string _bundleIdentifier;

        private List<string> _tags = new List<string>
        {
            "Player"
        };

        private List<string> _layers = new List<string>
        {
            "Camera",
            "Player",
            "Ground"
        };

        private List<string> _packages = new List<string>
        {
            "com.unity.textmeshpro",
            "com.unity.postprocessing"
        };

        private void OnEnable()
        {
            _headerStyle = new GUIStyle
            {
                normal = { textColor = Color.gray },
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };

            _labelStyle = new GUIStyle
            {
                normal = { textColor = Color.gray },
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleCenter
            };

            _companyName = Environment.UserName;
            _bundleIdentifier = $"com.{RemoveWhitespace(_companyName).ToLower()}.{RemoveWhitespace(PlayerSettings.productName).ToLower()}";
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                using (var scroll = new EditorGUILayout.ScrollViewScope(_scrollPosition))
                {
                    _scrollPosition = scroll.scrollPosition;
                    
                    DrawPlayerSettings();
                    EditorGUILayout.Space();

                    DrawList("Tags", "Tag", _tags);
                    EditorGUILayout.Space();

                    DrawList("Layers", "Layer", _layers);
                    EditorGUILayout.Space();

                    DrawList("Packages", "Package", _packages);
                    EditorGUILayout.Space();
                }

                if (GUILayout.Button("Setup", GUILayout.Height(30f)))
                {
                    Setup();
                }

                EditorGUILayout.Space();
            }
        }

        private void Setup()
        {
            foreach (var tag in _tags)
            {
                GameSetup.AddTag(tag);
            }

            foreach (var layer in _layers)
            {
                GameSetup.AddLayer(layer);
            }

            foreach (var package in _packages)
            {
                Client.Add(package);
            }

            PlayerSettings.companyName = _companyName;
            PlayerSettings.SplashScreen.show = _showSplashScreen;
            PlayerSettings.defaultInterfaceOrientation = _orientation;
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, _bundleIdentifier);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, _bundleIdentifier);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, _bundleIdentifier);

            EditorApplication.ExecuteMenuItem("File/Save Project");
        }

        private void DrawPlayerSettings()
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField("Player Settings", _headerStyle);

                _orientation = (UIOrientation)EditorGUILayout.EnumPopup("Orientation", _orientation);
                _showSplashScreen = EditorGUILayout.Toggle("Splash Screen", _showSplashScreen);
                _companyName = EditorGUILayout.TextField("Company Name", _companyName);
                _bundleIdentifier = EditorGUILayout.TextField("Bundle Identifier", _bundleIdentifier);
            }
        }

        private void DrawList(string title, string subtitle, List<string> list)
        {
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                EditorGUILayout.LabelField(title, _headerStyle);

                if (GUILayout.Button("Add"))
                {
                    list.Add(string.Empty);
                }

                for (var i = 0; i < list.Count; i++)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        list[i] = EditorGUILayout.TextField($"{subtitle} {i + 1}", list[i]);

                        if (GUILayout.Button("X", GUILayout.Width(25f)))
                        {
                            list.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        private static string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !char.IsWhiteSpace(c))
                .ToArray());
        }
    }
}