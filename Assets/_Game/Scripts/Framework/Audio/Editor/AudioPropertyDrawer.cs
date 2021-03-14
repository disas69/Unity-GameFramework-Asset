using Framework.Audio.Configuration;
using Framework.Editor;
using UnityEditor;

namespace Framework.Audio.Editor
{
    [CustomPropertyDrawer(typeof(Audio))]
    public class AudioPropertyDrawer : StorageItemPropertyDrawer<Audio, AudioStorage>
    {
    }
}