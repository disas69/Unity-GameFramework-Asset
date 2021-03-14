using UnityEngine;

namespace Source.Data
{
    public static class GameData
    {
        private const string LevelIndexKey = "Level_Index";
        private const string LevelProgressionKey = "Level_Progression";
        private const string TutorialKey = "Tutorial";

        public static int LevelIndex
        {
            get => PlayerPrefs.GetInt(LevelIndexKey, 1);
            set => PlayerPrefs.SetInt(LevelIndexKey, value);
        }

        public static int LevelProgression
        {
            get => PlayerPrefs.GetInt(LevelProgressionKey, 1);
            set => PlayerPrefs.SetInt(LevelProgressionKey, value);
        }

        public static bool IsTutorialComplete
        {
            get => PlayerPrefs.GetInt(TutorialKey, 0) == 1;
            set => PlayerPrefs.SetInt(TutorialKey, value ? 1 : 0);
        }

        public static void IncreaseLevel(bool increaseProgression)
        {
            LevelIndex++;
            if (increaseProgression)
            {
                LevelProgression++;
            }
        }

        public static void Reset()
        {
            LevelIndex = 1;
            LevelProgression = 1;
            IsTutorialComplete = false;
        }

        public static void Save()
        {
            PlayerPrefs.Save();
        }
    }
}