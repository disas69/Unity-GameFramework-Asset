using System.Collections.Generic;
using UnityEngine;

namespace Framework.Tools.Misc
{
    public class SkinRandomizer : MonoBehaviour
    {
        [SerializeField] private Transform _root;
        [SerializeField] private List<Material> _skins;

        private void OnEnable()
        {
            Apply();
        }

        public void Apply()
        {
            var skin = _skins[Random.Range(0, _skins.Count)];
            var renderers = _root.GetComponentsInChildren<Renderer>();

            foreach (var renderer in renderers)
            {
                renderer.material = skin;
            }
        }
    }
}