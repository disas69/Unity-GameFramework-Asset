using UnityEngine;
using UnityEngine.Events;

namespace Framework.Tools.Misc
{
    public class BoolEventSwitch : MonoBehaviour
    {
        public UnityEvent OnTrue;
        public UnityEvent OnFalse;

        public void Switch(bool value)
        {
            if (value)
            {
                OnTrue.Invoke();
            }
            else
            {
                OnFalse.Invoke();
            }
        }
    }
}