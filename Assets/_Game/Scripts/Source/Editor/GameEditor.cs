using System.IO;
using Source.Data;
using Source.Level;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace Source.Editor
{
    public static class GameEditor
    {
        public const string MainScenePath = "Assets/_Game/Scenes/Main.unity";

        [InitializeOnLoadMethod]
        public static void Initialize()
        {
            EditorApplication.playModeStateChanged += mode =>
            {
                if (mode == PlayModeStateChange.ExitingEditMode)
                {
                    LoadScene(MainScenePath);
                }
            };
        }

        [MenuItem("Game/Play", priority = 1)]
        public static void PlayGame()
        {
            EditorSceneManager.OpenScene(MainScenePath);
            EditorApplication.isPlaying = true;
        }

        [MenuItem("Game/Configuration", priority = 2)]
        public static void OpenGameConfiguration()
        {
            var window = EditorWindow.GetWindow<GameConfigurationWindow>("Game Configuration");
            window.minSize = new Vector2(350f, 450f);
            window.Show();
        }

        [MenuItem("Game/Reset Data", priority = 3)]
        public static void ClearGameData()
        {
            GameData.Reset();
            GameData.Save();
        }

        [MenuItem("Game/Toggle UI", priority = 4)]
        public static void ToggleUI()
        {
            var canvas = Object.FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                var canvasGroup = canvas.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    if (canvasGroup.alpha >= 1)
                    {
                        canvasGroup.alpha = 0;
                    }
                    else
                    {
                        canvasGroup.alpha = 1;
                    }
                }
            }
        }

        [MenuItem("Game/Setup", priority = 5)]
        public static void Setup()
        {
            var window = EditorWindow.GetWindow<GameSetupWindow>("Game Setup");
            window.minSize = new Vector2(350f, 450f);
            window.Show();
        }

        private static void LoadScene(string path)
        {
            var needToLoadScene = true;

            for (var i = 0; i < EditorSceneManager.sceneCount; i++)
            {
                var scene = EditorSceneManager.GetSceneAt(i);
                if (scene.name == Path.GetFileNameWithoutExtension(path))
                {
                    needToLoadScene = false;
                }
                else
                {
                    var roots = scene.GetRootGameObjects();
                    for (var j = 0; j < roots.Length; j++)
                    {
                        var levelController = roots[i].GetComponent<LevelController>();
                        if (levelController != null)
                        {
                            GameData.LevelIndex = scene.buildIndex;
                            GameData.LevelProgression = scene.buildIndex;
                            GameData.Save();
                        }
                    }

                    EditorSceneManager.CloseScene(scene, true);
                }
            }

            if (needToLoadScene)
            {
                EditorSceneManager.OpenScene(path, OpenSceneMode.Single);
            }
        }
    }
}