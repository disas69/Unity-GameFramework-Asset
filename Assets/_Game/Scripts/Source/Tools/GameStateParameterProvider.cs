using Framework.Signals.ParameterProviders;
using Source.State;

namespace Source.Tools
{
    public class GameStateParameterProvider : IntParameterProvider
    {
        public GameState GameState;

        public override int GetValue()
        {
            return (int) GameState;
        }
    }
}