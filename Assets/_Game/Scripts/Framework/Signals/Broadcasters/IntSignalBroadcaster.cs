using Framework.Signals.ParameterProviders;

namespace Framework.Signals.Broadcasters
{
    public class IntSignalBroadcaster : SignalBroadcaster<int, IntParameterProvider>
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

        public override void Broadcast(int parameter)
        {
            SignalsManager.Broadcast(Signal.Name, parameter);
        }

        public override void Broadcast(IntParameterProvider parameterProvider)
        {
            SignalsManager.Broadcast(Signal.Name, parameterProvider.GetValue());
        }
    }
}