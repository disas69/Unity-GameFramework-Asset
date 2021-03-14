using System;
using System.Collections;
using Framework.Tasking;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Level
{
    public class LevelLoader
    {
        private readonly bool _isAsync;

        private LevelController _levelController;
        private LevelConfiguration _configuration;

        public int Level => _configuration?.Level ?? -1;
        public bool IsLoaded { get; private set; }

        public LevelLoader(bool isAsync = false)
        {
            _isAsync = isAsync;
        }

        public void Load(LevelConfiguration configuration, Action<LevelController> callback)
        {
            if (IsLoaded)
            {
                Debug.LogError("Attempt to load new level scene while previous was not unloaded. This behaviour is not supported");
                return;
            }

            _configuration = configuration;

            if (!string.IsNullOrEmpty(configuration.Scene))
            {
                Task.Create(LoadLevel(configuration.Level, _isAsync))
                    .Subscribe(() =>
                    {
                        var level = UnityEngine.Object.FindObjectOfType<LevelController>();
                        if (level != null)
                        {
                            _levelController = level;
                            IsLoaded = true;
                            callback?.Invoke(level);
                        }
                    })
                    .Start();
            }
            else
            {
                Debug.LogError($"Scene asset at index {configuration.Level} is missing");
            }
        }

        public void Unload(Action callback)
        {
            if (IsLoaded && _configuration != null && _levelController != null)
            {
                _levelController.Dispose();

                Task.Create(UnloadLevel(_configuration.Level, _isAsync))
                    .Subscribe(() =>
                    {
                        _configuration = null;
                        _levelController = null;
                        IsLoaded = false;
                        callback?.Invoke();
                    })
                    .Start();
            }
            else
            {
                Debug.LogError("Attempt to unload level scene while there are no loaded scenes");
            }
        }

        private static IEnumerator LoadLevel(int scene, bool async)
        {
            if (async)
            {
                yield return SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            }
        }

        private static IEnumerator UnloadLevel(int scene, bool async)
        {
            if (async)
            {
                yield return SceneManager.UnloadSceneAsync(scene);
            }
            else
            {
                SceneManager.UnloadScene(scene);
            }
        }
    }
}
