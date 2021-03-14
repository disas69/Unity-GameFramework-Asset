using System;
using Source.Player;
using UnityEngine;

namespace Source.Level
{
    public class LevelProgress
    {
        private readonly LevelController _level;
        private readonly PlayerController _player;

        public int CurrentLevel { get; private set; }
        public int NextLevel { get; private set; }
        public float Value { get; private set; }

        public event Action<float> Changed;

        public LevelProgress(int currentLevel, LevelController level, PlayerController player)
        {
            _level = level;
            _player = player;

            CurrentLevel = currentLevel;
            NextLevel = currentLevel + 1;
            Value = 0f;
        }

        public void Update()
        {
            var levelDistance = Vector3.Distance(_level.Start.position, _level.Finish.position);
            var passedDistance = Vector3.Distance(_level.Start.position, _player.transform.position);

            var value = passedDistance / levelDistance;

            if (Math.Abs(value - Value) > 0.01f)
            {
                Value = value;
                Changed?.Invoke(value);
            }
        }
    }
}