using System.Collections;
using System.Collections.Generic;
using Live2D.Cubism.Rendering.Masking;
using UnityEngine;

namespace Dedalord.LiveAr
{
    public class Live2DMaskController : Live2DControllerElement
    {
        private Live2DMaskProvider _provider;
        private CubismMaskController _maskController;
        private int _maskIdentifier;

        private void Awake()
        {
            _provider ??= FindObjectOfType<Live2DMaskProvider>();
            _maskController ??= Controller.GetComponentInChildren<CubismMaskController>();
        }
        
        private void OnEnable()
        {
            GetMaskTexture();
        }

        private void OnDisable()
        {
            _provider?.Return(_maskIdentifier);
        }

        private void GetMaskTexture()
        {
            if (_provider == null)
            {
                Debug.LogWarning("A Live2DMaskProvider is required in the scene to prevent masking issues! Using default mask texture");
                _maskController.MaskTexture = CubismMaskTexture.GlobalMaskTexture;
                return;
            }
            _maskIdentifier = _provider.Get(out var assignedTexture);
            _maskController.MaskTexture = assignedTexture;
        }
    }
}
