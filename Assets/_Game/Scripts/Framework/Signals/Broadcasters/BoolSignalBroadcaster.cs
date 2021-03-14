using Framework.Signals.ParameterProviders;

namespace Framework.Signals.Broadcasters
{
    public class BoolSignalBroadcaster : SignalBroadcaster<bool, BoolParameterProvider>
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

        public override void Broadcast(bool parameter)
        {
            SignalsManager.Broadcast(Signal.Name, parameter);
        }

        public override void Broadcast(BoolParameterProvider parameterProvider)
        {
            SignalsManager.Broadcast(Signal.Name, parameterProvider.GetValue());
        }
    }
}