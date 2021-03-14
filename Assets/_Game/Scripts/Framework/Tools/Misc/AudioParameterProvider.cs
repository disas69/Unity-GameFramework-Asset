using Framework.Signals.ParameterProviders;

namespace Framework.Tools.Misc
{
    public class AudioParameterProvider : StringParameterProvider
    {
        public Framework.Audio.Audio Audio;

        public override string GetValue()
        {
            return Audio.Name;
        }
    }
}