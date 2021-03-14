using UnityEngine;

namespace Source.Tools
{
    public class ActivatableMonoBehaviour : MonoBehaviour
    {
        public bool IsActive { get; private set; }

        public virtual void Activate(bool isActive)
        {
            IsActive = isActive;
        }
    }
}