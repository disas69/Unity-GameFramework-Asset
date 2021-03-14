using UnityEngine;

namespace Source.Tools
{
    [RequireComponent(typeof(Renderer))]
    public class TextureScroller : MonoBehaviour
    {
        private Renderer _renderer;

        [SerializeField] private Vector2 _speed;

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            var offsetX = Time.time * _speed.x;
            var offsetY = Time.time * _speed.y;
            _renderer.material.SetTextureOffset("_MainTex", new Vector2(offsetX, offsetY));
        }
    }
}