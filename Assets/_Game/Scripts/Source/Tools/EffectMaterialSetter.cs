using System.Collections.Generic;
using UnityEngine;

namespace Source.Effects
{
    public class EffectMaterialSetter : MonoBehaviour
    {
        [SerializeField] private List<ParticleSystemRenderer> _renderers;

        public void Set(Material material)
        {
            for (var i = 0; i < _renderers.Count; i++)
            {
                _renderers[i].sharedMaterial = material;
            }
        }
    }
}
