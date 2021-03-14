using Source.Configuration;
using UnityEditor;

namespace Source.Editor
{
    public class GameConfigurationWindow : EditorWindow
    {
        private UnityEditor.Editor _editor;

        private void OnEnable()
        {
            _editor = UnityEditor.Editor.CreateEditor(GameConfiguration.Instance);
        }

        private void OnGUI()
        {
            if (_editor != null)
            {
                _editor.OnInspectorGUI();
            }
        }
    }
}