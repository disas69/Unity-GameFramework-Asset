using TMPro;
using UnityEngine;

namespace Framework.Localization
{
    public enum TextLocalizerMode
    {
        NoMode,
        ToUpper,
        ToLower
    }

    [RequireComponent(typeof(TMP_Text))]
    public class TextLocalizer : MonoBehaviour
    {
        private TMP_Text _textComponent;

        [HideInInspector] public string Key;
        public TextLocalizerMode Mode = TextLocalizerMode.NoMode;

        private void Start()
        {
            _textComponent = GetComponent<TMP_Text>();

            LocalizationManager.LanguageChanged += Localize;
            Localize();
        }

        private void Localize()
        {
            if (_textComponent != null)
            {
                _textComponent.text = GetString(Key);
            }
        }

        private string GetString(string key)
        {
            var result = LocalizationManager.GetString(key);

            switch (Mode)
            {
                case TextLocalizerMode.ToUpper:
                    result = result.ToUpper();
                    break;
                case TextLocalizerMode.ToLower:
                    result = result.ToLower();
                    break;
            }

            return result;
        }

        private void OnDestroy()
        {
            LocalizationManager.LanguageChanged -= Localize;
        }
    }
}