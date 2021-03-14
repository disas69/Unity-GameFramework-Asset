using System.Collections.Generic;
using Framework.Attributes;
using Framework.Tools.Singleton;
using Source.Data;
using Source.Level;
using Source.Player;
using UnityEngine;

namespace Source.Configuration
{
    [ResourcePath(AssetPath)]
    [CreateAssetMenu(fileName = "GameConfiguration")]
    public class GameConfiguration : ScriptableSingleton<GameConfiguration>
    {
        public const string AssetPath = "GameConfiguration";

        public GameSettings Game;
        public PlayerSettings Player;
        public List<LevelConfiguration> Levels;

        public static GameSettings GameSettings => Instance.Game;
        public static PlayerSettings PlayerSettings => Instance.Player;

        public static LevelConfiguration GetLevelConfiguration(int level)
        {
            if (Instance.Levels.Count == 0)
            {
                return null;
            }
            
            level %= Instance.Levels.Count;

            var levelConfiguration = Instance.Levels.Find(l => l.Level == level);
            if (levelConfiguration != null)
            {
                return levelConfiguration;
            }

            return Instance.Levels[Instance.Levels.Count - 1];
        }

        public static bool IsTutorialLevel(int level)
        {
            var configuration = GetLevelConfiguration(level);
            if (configuration != null)
            {
                return configuration.Settings.IsTutorial;
            }

            return false;
        }
    }
}