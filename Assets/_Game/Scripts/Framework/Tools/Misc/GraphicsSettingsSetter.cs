using UnityEngine;

namespace Framework.Tools.Misc
{
    public class GraphicsSettingsSetter : MonoBehaviour
    {
        public int QualityLevel = 0;
        [Range(30, 60)] public int FrameRate = 60;
        public bool VSync = false;

        private void Awake()
        {
            Application.targetFrameRate = FrameRate;
            QualitySettings.SetQualityLevel(QualityLevel);
            QualitySettings.vSyncCount = VSync ? 1 : 0;
        }
    }
}