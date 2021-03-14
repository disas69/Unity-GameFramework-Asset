using Framework.Signals.ParameterProviders;
using UnityEngine;

namespace Framework.Signals.Broadcasters
{
    public class Vector3SignalBroadcaster : SignalBroadcaster<Vector3, Vector3ParameterProvider>
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

        public override void Broadcast(Vector3 parameter)
        {
            SignalsManager.Broadcast(Signal.Name, parameter);
        }

        public override void Broadcast(Vector3ParameterProvider parameterProvider)
        {
            SignalsManager.Broadcast(Signal.Name, parameterProvider.GetValue());
        }
    }
}