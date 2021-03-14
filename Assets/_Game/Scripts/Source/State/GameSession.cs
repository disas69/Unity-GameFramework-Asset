using System;
using Framework.Effects;
using Framework.Extensions;
using Source.Camera;
using Source.Configuration;
using Source.Data;
using Source.Level;
using Source.Player;
using Source.Spawn;
using Source.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Source.State
{
    public class GameSession : MonoBehaviour, IDisposable
    {
        [SerializeField] private CameraController _camera;
        [SerializeField] private PlayerController _player;
        [SerializeField] private UnityEvent _onSuccess;
        [SerializeField] private UnityEvent _onFailure;

        public CameraController Camera => _camera;
        public PlayerController Player => _player;
        public LevelController Level { get; private set; }
        private LevelLoader LevelLoader { get; set; }

        private void Awake()
        {
            LevelLoader = new LevelLoader(true);
        }

        public void Initialize(int level)
        {
            Camera.Initialize();
            Camera.Activate(false);

            Player.gameObject.SetActive(false);
            Player.Activate(false);

            var configuration = GameConfiguration.GetLevelConfiguration(level);
            if (configuration != null)
            {
                LevelLoader.Load(configuration, levelController =>
                {
                    Level = levelController;

                    Player.transform.position = Level.Start.position;
                    Player.gameObject.SetActive(true);
                    Player.Activate(false);
                    Player.Initialize();

                    Level.Initialize(level, Player, configuration);

                    Camera.ResetState();
                    Camera.Focus(0, true);
                });
            }
        }

        public void Play()
        {
            Camera.UnFocus();
            Camera.Activate(true);

            Level.Activate(true);
            Player.Activate(true);

            GameAnalytics.OnGameStarted(LevelLoader.Level);
        }

        public void Stop(bool isSuccess)
        {
            Level.Activate(false);
            Player.Activate(false);

            if (isSuccess)
            {
                _onSuccess.Invoke();
                Player.PlaySuccess();

                if (GameData.IsTutorialComplete)
                {
                    if (GameConfiguration.IsTutorialLevel(Level.Level + 1))
                    {
                        GameData.IncreaseLevel(false);
                    }
                }
                else
                {
                    if (GameConfiguration.IsTutorialLevel(Level.Level))
                    {
                        GameData.IsTutorialComplete = true;
                    }
                }

                GameData.IncreaseLevel(true);
            }
            else
            {
                _onFailure.Invoke();
                Player.PlayFailure();
            }

            GameData.Save();
            GameAnalytics.OnGameFinished(LevelLoader.Level, isSuccess);
        }

        public void WaitUntilLoaded(Action callback)
        {
            this.WaitUntil(() => LevelLoader.IsLoaded, callback);
        }

        public void WaitUntilUnloaded(Action callback)
        {
            this.WaitUntil(() => !LevelLoader.IsLoaded, callback);
        }

        public void Dispose()
        {
            Player.Dispose();
            VisualEffectsManager.Clear();
            LevelElementsSpawner.Clear();
            LevelLoader.Unload(GC.Collect);
        }
    }
}