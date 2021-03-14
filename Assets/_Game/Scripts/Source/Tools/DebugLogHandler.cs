using UnityEngine;

namespace Source.Tools
{
    public class DebugLogHandler : MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.unityLogger.logEnabled = true;
#else
            Debug.unityLogger.logEnabled = false;
#endif
        }
    }
}