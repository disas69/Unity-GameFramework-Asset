using Source.Data;

public partial class SROptions
{
    public int LevelIndex
    {
        get => GameData.LevelIndex;
        set => GameData.LevelIndex = value;
    }

    public int LevelProgression
    {
        get => GameData.LevelProgression;
        set => GameData.LevelProgression = value;
    }
}