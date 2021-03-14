using TMPro;
using UnityEngine;

namespace Framework.Effects
{
    public class TextEffect : AnimationEffect
    {
        [SerializeField] private TextMeshPro _textMeshPro;

        public void Initialize(string text)
        {
            _textMeshPro.text = text;
        }
    }
}