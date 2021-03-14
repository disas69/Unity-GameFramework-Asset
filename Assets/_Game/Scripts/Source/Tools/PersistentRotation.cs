using UnityEngine;

namespace Source.Tools
{
    public class PersistentRotation : MonoBehaviour
    {
        [SerializeField] private Vector3 _lookRotation;

        private void Update()
        {
            transform.rotation = Quaternion.LookRotation(_lookRotation, Vector3.up);
        }
    }
}