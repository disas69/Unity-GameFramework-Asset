using Framework.Editor;
using UnityEditor;

namespace Framework.Signals.Editor
{
    [CustomEditor(typeof(SignalsStorage))]
    public class SignalsStorageEditor : StorageEditor<Signal, SignalsStorage>
    {
    }
}