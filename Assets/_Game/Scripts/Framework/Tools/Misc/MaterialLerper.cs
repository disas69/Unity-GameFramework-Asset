using System.Collections.Generic;
using UnityEngine;

namespace Framework.Tools.Misc
{
    public class MaterialLerper : MonoBehaviour
    {
        [SerializeField] private Material _from;
        [SerializeField] private Material _to;
        [SerializeField] private AnimationCurve _curve;
        [SerializeField] private List<Renderer> _renderers;

        public void Lerp(float value)
        {
            Lerp(_from, _to, value);
        }

        public void Lerp(Material from, Material to, float value)
        {
            for (var i = 0; i < _renderers.Count; i++)
            {
                _renderers[i].material.Lerp(from, to, _curve.Evaluate(value));
            }
        }

        public void Reset()
        {
            for (var i = 0; i < _renderers.Count; i++)
            {
                _renderers[i].material = _from;
            }
        }
    }
}