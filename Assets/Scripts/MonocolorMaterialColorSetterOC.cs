using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [ExecuteInEditMode]
    public class MonocolorMaterialColorSetterOC : MonoBehaviour
    {
        public Color Color;
        private List<MaterialPropertyBlock> _propBlocks;
        private List<Renderer> _renderers;

        public void Awake()
        {
            AffirmRenderingFieldsAreSet();
        }

        public void OnValidate()
        {
            AffirmRenderingFieldsAreSet();
            _renderers.Select((c, i) => new {renderer=c, index=i}).ToList().ForEach(c => 
            {
                var block = _propBlocks[c.index];
                c.renderer.GetPropertyBlock(block);
                block.SetColor("_Color", Color);
                c.renderer.SetPropertyBlock(block);
            });
        }

        private void AffirmRenderingFieldsAreSet()
        {
            if (_renderers == null)
            {
                _renderers = GetComponentsInChildren<Renderer>().ToList();
                var outRenderer = GetComponent<Renderer>();
                if (outRenderer != null)
                {
                    _renderers.Add(outRenderer);

                }
            }

            if (_propBlocks == null)
            {
                _propBlocks = Enumerable.Range(0, _renderers.Count).Select(c => new MaterialPropertyBlock()).ToList();
            }
        }
    }
}
