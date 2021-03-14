using System.Collections.Generic;
using Framework.Signals.Broadcasters;
using Framework.Signals.Listeners;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework.Signals.Editor
{
    public class SignalsBrowser : EditorWindow
    {
        public class SignalState
        {
            public bool IsFolded;
            public List<ISignalBroadcaster> Broadcasters;
            public List<ISignalListener> Listeners;
        }

        private readonly Dictionary<string, SignalState> _signalStates = new Dictionary<string, SignalState>();
        private Vector2 _scrollPosition;
        private GUIStyle _headerStyle;
        private GUIStyle _labelStyle;

        [MenuItem("Signals/Browser")]
        public static void Open()
        {
            var window = EditorWindow.GetWindow<SignalsBrowser>("Signals Browser");
            window.minSize = new Vector2(370f, 470f);
            window.Show();
        }

        private void OnEnable()
        {
            _headerStyle = new GUIStyle
            {
                normal = {textColor = Color.gray},
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };
            _labelStyle = new GUIStyle
            {
                normal = {textColor = Color.gray},
                fontStyle = FontStyle.Italic,
                alignment = TextAnchor.MiddleLeft
            };
        }

        private void OnGUI()
        {
            using (new EditorGUILayout.VerticalScope())
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    if (GUILayout.Button("Inspect Scene"))
                    {
                        InspectScene();
                    }

                    if (GUILayout.Button("Inspect Project"))
                    {
                        InspectProject();
                    }
                }

                using (var scroll = new EditorGUILayout.ScrollViewScope(_scrollPosition))
                {
                    _scrollPosition = scroll.scrollPosition;
                    DrawSignalStates();
                }
            }
        }

        private void InspectScene()
        {
            _signalStates.Clear();

            var scene = SceneManager.GetActiveScene();
            var roots = scene.GetRootGameObjects();

            foreach (var root in roots)
            {
                ProcessGameObject(root);
            }
        }

        private void InspectProject()
        {
            _signalStates.Clear();

            var prefabs = AssetDatabase.FindAssets("t:prefab");

            foreach (var prefab in prefabs)
            {
                var path = AssetDatabase.GUIDToAssetPath(prefab);
                ProcessGameObject(AssetDatabase.LoadAssetAtPath<GameObject>(path));
            }

            var scenes = AssetDatabase.FindAssets("t:scene");

            foreach (var scene in scenes)
            {
                var path = AssetDatabase.GUIDToAssetPath(scene);
                var sceneAdditive = EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
                var roots = sceneAdditive.GetRootGameObjects();

                foreach (var root in roots)
                {
                    ProcessGameObject(root);
                }
            }
        }

        private void ProcessGameObject(GameObject root)
        {
            var broadcasters = root.GetComponentsInChildren<ISignalBroadcaster>();
            foreach (var broadcaster in broadcasters)
            {
                SignalState state;
                if (_signalStates.TryGetValue(broadcaster.SignalName, out state))
                {
                    if (!state.Broadcasters.Contains(broadcaster))
                    {
                        state.Broadcasters.Add(broadcaster);
                    }
                }
                else
                {
                    _signalStates.Add(broadcaster.SignalName, new SignalState
                    {
                        Broadcasters = new List<ISignalBroadcaster> {broadcaster},
                        Listeners = new List<ISignalListener>()
                    });
                }
            }

            var listeners = root.GetComponentsInChildren<ISignalListener>();
            foreach (var listener in listeners)
            {
                SignalState state;
                if (_signalStates.TryGetValue(listener.SignalName, out state))
                {
                    if (!state.Listeners.Contains(listener))
                    {
                        state.Listeners.Add(listener);
                    }
                }
                else
                {
                    _signalStates.Add(listener.SignalName, new SignalState
                    {
                        Broadcasters = new List<ISignalBroadcaster>(),
                        Listeners = new List<ISignalListener> {listener}
                    });
                }
            }
        }

        private void DrawSignalStates()
        {
            if (_signalStates.Count == 0)
            {
                EditorGUILayout.LabelField("No Signals", _labelStyle);
                return;
            }
            
            using (new EditorGUILayout.VerticalScope(GUI.skin.box))
            {
                foreach (var state in _signalStates)
                {
                    using (new EditorGUILayout.VerticalScope(GUI.skin.box))
                    {
                        EditorGUILayout.LabelField($"Signal: {state.Key}", _headerStyle);
                        EditorGUILayout.LabelField("Broadcasters", _labelStyle);

                        foreach (var broadcaster in state.Value.Broadcasters)
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.LabelField($"{broadcaster.GameObject.name} ({broadcaster.GetType().Name})");

                                if (GUILayout.Button("Select", GUILayout.Width(70)))
                                {
                                    Selection.activeObject = broadcaster.GameObject;
                                }
                            }
                        }
                    
                        EditorGUILayout.LabelField("Listeners", _labelStyle);

                        foreach (var listener in state.Value.Listeners)
                        {
                            using (new EditorGUILayout.HorizontalScope())
                            {
                                state.Value.IsFolded = EditorGUILayout.Foldout(state.Value.IsFolded, $"{listener.GameObject.name} ({listener.GetType().Name})");

                                if (GUILayout.Button("Select", GUILayout.Width(70)))
                                {
                                    Selection.activeObject = listener.GameObject;
                                }
                            }

                            if (state.Value.IsFolded)
                            {
                                EditorGUI.indentLevel++;
                                {
                                    foreach (var action in listener.Actions)
                                    {
                                        EditorGUILayout.LabelField(action, _labelStyle);
                                    }
                                }
                                EditorGUI.indentLevel--;
                            }
                        }
                    }
                }
            }
        }
    }
}