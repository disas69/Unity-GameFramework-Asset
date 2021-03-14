using System;

namespace Source.Level
{
    [Serializable]
    public class LevelConfiguration
    {
        public int Level;
        public string Scene;
        public LevelSettings Settings;
    }

    [Serializable]
    public class LevelSettings
    {
        public bool IsTutorial;
    }
}