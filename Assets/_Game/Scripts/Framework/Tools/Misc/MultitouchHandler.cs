using UnityEngine;

namespace Framework.Tools.Misc
{
    public class MultitouchHandler : MonoBehaviour
    {
        [SerializeField] private bool _isEnabled;

        private void Awake()
        {
            Enable(_isEnabled);
        }

        public static void Enable(bool isEnabled)
        {
            UnityEngine.Input.multiTouchEnabled = isEnabled;
        }
    }
}