using Framework.Signals.ParameterProviders;

namespace Framework.Signals.Broadcasters
{
    public class FloatSignalBroadcaster : SignalBroadcaster<float, FloatParameterProvider>
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

        public override void Broadcast(float parameter)
        {
            SignalsManager.Broadcast(Signal.Name, parameter);
        }

        public override void Broadcast(FloatParameterProvider parameterProvider)
        {
            SignalsManager.Broadcast(Signal.Name, parameterProvider.GetValue());
        }
    }
}