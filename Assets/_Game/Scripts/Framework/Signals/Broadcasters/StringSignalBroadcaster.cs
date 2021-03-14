using Framework.Signals.ParameterProviders;

namespace Framework.Signals.Broadcasters
{
    public class StringSignalBroadcaster : SignalBroadcaster<string, StringParameterProvider>
    {
        public override void Broadcast()
        {
            if (IsParameterProvided && Provider != null)
            {
                SignalsManager.Broadcast(Signal.Name, Provider.GetValue());
            }
            else
            {
                SignalsManager.Broadcast(Signal.Name, Parameter);
            }
        }

        public override void Broadcast(string parameter)
        {
            SignalsManager.Broadcast(Signal.Name, parameter);
        }

        public override void Broadcast(StringParameterProvider parameterProvider)
        {
            SignalsManager.Broadcast(Signal.Name, parameterProvider.GetValue());
        }
    }
}