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
        private MaterialPropertyBlock _propBlock;
        private Renderer _renderer;

        public void Awake()
        {
            AffirmRenderingFieldsAreSet();
        }

        public void OnValidate()
        {
            AffirmRenderingFieldsAreSet();
            _renderer.GetPropertyBlock(_propBlock);
            _propBlock.SetColor("_Color", Color);
            _renderer.SetPropertyBlock(_propBlock);
        }

        private void AffirmRenderingFieldsAreSet()
        {
            if (_propBlock == null)
            {
                _propBlock = new MaterialPropertyBlock();
            }

            if (_renderer == null)
            {
                _renderer = GetComponent<Renderer>();
            }
        }
    }
}
