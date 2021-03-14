using Framework.Editor;
using UnityEditor;

namespace Framework.Signals.Editor
{
    [CustomPropertyDrawer(typeof(Signal))]
    public class SignalPropertyDrawer : StorageItemPropertyDrawer<Signal, SignalsStorage>
    {
    }
}