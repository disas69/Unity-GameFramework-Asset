using Framework.Editor;
using UnityEditor;

namespace Framework.Effects.Editor
{
    [CustomPropertyDrawer(typeof(Effect))]
    public class EffectPropertyDrawer : StorageItemPropertyDrawer<Effect, EffectsStorage>
    {
    }
}