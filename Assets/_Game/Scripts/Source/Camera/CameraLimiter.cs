using UnityEngine;

namespace Source.Camera
{
    public enum CameraLimiterType
    {
        Horizontal,
        Vertical
    }

    [RequireComponent(typeof(Collider))]
    public class CameraLimiter : MonoBehaviour
    {
        private Collider _collider;

        [SerializeField] private CameraLimiterType _type = CameraLimiterType.Horizontal;

        public CameraLimiterType Type => _type;
        
        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }

        public Vector3 GetClosestPoint(Vector3 position)
        {
            return _collider.ClosestPoint(position);
        }
    }
}
