using System;
using UnityEngine;
using Source.State;
using Source.Player;
using Framework.Spawn;
using Source.Tools;

namespace Source.Level
{
    public class LevelController : ActivatableMonoBehaviour, IDisposable
    {
        private PlayerController _player;
        private LevelConfiguration _configuration;

        public Transform Start;
        public Transform Finish;
        public Transform Elements;

        public int Level { get; private set; }
        public LevelProgress Progress { get; private set; }

        public virtual void Initialize(int level, PlayerController player, LevelConfiguration configuration)
        {
            _player = player;
            _configuration = configuration;

            Level = level;
            Progress = new LevelProgress(level, this, player);
        }

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            Progress.Update();
        }

        public virtual void Dispose()
        {
            Activate(false);

            var spawnables = Elements.GetComponentsInChildren<SpawnableObject>(true);

            for (var i = 0; i < spawnables.Length; i++)
            {
                spawnables[i].Deactivate();
            }
        }
    }
}