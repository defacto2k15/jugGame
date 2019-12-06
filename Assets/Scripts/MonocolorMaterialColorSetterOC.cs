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
        public bool SetInChildren = true;
        public Color Color;
        private List<MaterialPropertyBlock> _propBlocks;
        private List<Renderer> _renderers;

        public void Awake()
        {
            ChangeColorsToMatchFieldValue();
        }

        public void OnValidate()
        {
            ChangeColorsToMatchFieldValue();
        }

        private void ChangeColorsToMatchFieldValue()
        {
            AffirmRenderingFieldsAreSet();
            _renderers.Select((c, i) => new {renderer = c, index = i}).ToList().ForEach(c =>
            {
                Debug.Log("Setting");
                var block = _propBlocks[c.index];
                c.renderer.GetPropertyBlock(block);
                block.SetColor("_Color", Color);
                c.renderer.SetPropertyBlock(block);
            });
        }

        public void SetColor(Color color)
        {
            Color = color;
            ChangeColorsToMatchFieldValue();
        }

        private void AffirmRenderingFieldsAreSet()
        {
            if (_renderers == null)
            {
                _renderers = new List<Renderer>();
                if (SetInChildren)
                {
                    _renderers.AddRange(GetComponentsInChildren<Renderer>().ToList());
                }
                var ourRenderer = GetComponent<Renderer>();
                if (ourRenderer != null)
                {
                    _renderers.Add(ourRenderer);

                }
            }

            if (_propBlocks == null)
            {
                _propBlocks = Enumerable.Range(0, _renderers.Count).Select(c => new MaterialPropertyBlock()).ToList();
            }
        }
    }
}
