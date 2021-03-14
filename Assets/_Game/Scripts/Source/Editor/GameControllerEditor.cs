using Framework.Editor;
using Framework.Editor.GUIUtilities;
using Source.State;
using UnityEditor;
using UnityEngine;

namespace Source.Editor
{
    [CustomEditor(typeof(GameController))]
    public class GameControllerEditor : CustomEditorBase<GameController>
    {
        private GameState _gameState;

        protected override void DrawInspector()
        {
            base.DrawInspector();
            DrawDefaultInspector();

            if (EditorApplication.isPlaying)
            {
                using (new GUIEnabled(false))
                {
                    EditorGUILayout.LabelField($"Current State: {Target.State}");
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    _gameState = (GameState) EditorGUILayout.EnumPopup("New State", _gameState);

                    if (GUILayout.Button("Set", GUILayout.Width(50f)))
                    {
                        Target.SetState(_gameState);
                    }
                }
            }
        }
    }
}