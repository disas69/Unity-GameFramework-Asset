using System.Collections;
using UnityEngine;

namespace Framework.Tools.Misc
{
    public class ColorLerper : MonoBehaviour
    {
        private float _elapsedTime;
        private Coroutine _coroutine;

        [SerializeField] private float _changeTime;
        [SerializeField] private Renderer _renderer;

        public void SetRenderer(Renderer renderer)
        {
            _renderer = renderer;
        }

        public void ToColor(Color color)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }

            _elapsedTime = 0f;
            _coroutine = StartCoroutine(ChangeColorCoroutine(_renderer.sharedMaterial.color, color, _changeTime));
        }

        private IEnumerator ChangeColorCoroutine(Color current, Color target, float changeTime)
        {
            while (_elapsedTime < changeTime)
            {
                _renderer.sharedMaterial.color = Color.Lerp(current, target, _elapsedTime / changeTime);
                _elapsedTime += Time.deltaTime;
                yield return null;
            }

            _renderer.sharedMaterial.color = target;
        }
    }
}